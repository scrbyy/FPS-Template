using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterEngine : MonoBehaviour
    {
        public Vector3 Velocity => _velocity;
        public bool IsImpulseActive => _isImpulseActive;

        [Header("Movement")]
        [SerializeField] private float _accelerationRate;
        [SerializeField] private float _decelerationRate;
        [SerializeField] private float _movingThreshold;

        [Header("CS-Style Air Movement")]
        [SerializeField] private float _airAcceleration;
        [SerializeField] private float _airCap;

        [Header("Gravity")]
        [SerializeField] private float _gravityScale;
        [SerializeField] private float _downforce;

        [Header("References")]
        [SerializeField] private CharacterSpeed _speedProvider;

        [Inject] private CharacterCollisionHandler _characterCollisionHandler;
        [Inject] private IGroundChecker _groundChecker;

        private CharacterController _characterController;

        private bool _isImpulseActive;
        private bool _canMove;
        private Vector3 _velocity;

        public void Move(Vector3 inputVector)
        {
            if (_canMove)
            {
                if (_groundChecker.IsGrounded) ApplyGroundMovement(inputVector, _speedProvider.Speed);
                else ApplyAirMovement(inputVector);

                Vector3 finalMotion = _velocity;

                _characterController.Move(finalMotion * Time.deltaTime);
            }
        }

        public void Jump(float height)
        {
            _velocity.y = Mathf.Sqrt(2f * height * Mathf.Abs(Physics.gravity.y * _gravityScale));
        }

        public void AddImpulse(Vector3 impulse)
        {
            _velocity += impulse;
            _isImpulseActive = true;
        }

        private void ApplyGroundMovement(Vector3 wishDirection, float speed)
        {
            float targetSpeed = wishDirection.magnitude * speed;

            if (targetSpeed > 0.01f) _isImpulseActive = false;

            float verticalVelocity = _velocity.y;

            if (_isImpulseActive)
            {
                Vector3 horizontalVel = new Vector3(_velocity.x, 0, _velocity.z);
                horizontalVel = Vector3.MoveTowards(horizontalVel, Vector3.zero, _decelerationRate * Time.deltaTime);

                _velocity = new Vector3(horizontalVel.x, verticalVelocity, horizontalVel.z);

                if (horizontalVel.magnitude <= speed) _isImpulseActive = false;
                return;
            }

            float accel = (targetSpeed > 0 ? _accelerationRate : _decelerationRate);

            Vector3 currentHorizontal = new Vector3(_velocity.x, 0, _velocity.z);
            Vector3 targetHorizontal = wishDirection * targetSpeed;

            Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetHorizontal, accel * Time.deltaTime);

            _velocity = new Vector3(newHorizontal.x, verticalVelocity, newHorizontal.z);
        }

        private void ApplyAirMovement(Vector3 wishDirection)
        {
            if (wishDirection.magnitude <= 0) return;

            float currentSpeedInWishDir = Vector3.Dot(_velocity, wishDirection);
            float addSpeed = Mathf.Max(0, _airCap - currentSpeedInWishDir);

            if (addSpeed > 0)
            {
                float accelSpeed = Mathf.Min(_airAcceleration * Time.deltaTime, addSpeed);
                _velocity += wishDirection * accelSpeed;
            }
        }

        private void ApplyGravity()
        {
            if (_groundChecker.IsGrounded && _velocity.y < 0f)
                _velocity.y = _downforce;
            else
                _velocity.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
        }

        public void DisableMovement()
        {
            _canMove = false;
        }

        public void EnableMovement()
        {
            _canMove = true;
        }

        public bool IsMoving()
        {
            Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
            return horizontalVelocity.magnitude > _movingThreshold;
        }

        private void Update()
        {
            if (_canMove)
            {
                ApplyGravity();
            }
            HandleCollisions();
        }

        private void HandleCollisions()
        {
            if (_characterCollisionHandler.IsCollisionedAbove() && _velocity.y > 0f)
            {
                _velocity.y = 0f;
            }

            if (_isImpulseActive && _characterCollisionHandler.IsCollisionedBySide())
            {
                _velocity.x = 0f;
                _velocity.z = 0f;

                _isImpulseActive = false;
            }
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _canMove = true;
        }
    }
}