using UnityEngine;

public class CurrentVelocityText : UpdateableText
{
    [SerializeField] private CharacterEngine _characterEngine;

    protected override void UpdateText()
    {
        Vector3 vel = _characterEngine.Velocity;

        float speed = Mathf.Sqrt(vel.x * vel.x + vel.z * vel.z);
        _text.SetText("Current Velocity: {0:1}", speed);
    }
}