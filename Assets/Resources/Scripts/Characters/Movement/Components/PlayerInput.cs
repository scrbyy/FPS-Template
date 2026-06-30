using Zenject;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IMovementInputProvider _inputProvider;

    private void Update()
    {
        Vector2 input = _inputProvider.MoveInput;
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;
        _characterEngine.Move(wishDir);
    }
}