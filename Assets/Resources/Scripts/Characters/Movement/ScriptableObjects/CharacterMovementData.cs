using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementData", menuName = "FPS Template/Data Assets/Movement/CharacterMovement Data Asset")]
public class CharacterMovementData : ScriptableObject
{
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float CrouchSpeed => _crouchSpeed;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _crouchSpeed;
}
