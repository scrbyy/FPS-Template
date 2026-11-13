using UnityEngine;

using scrbyy.FpsTemplate.Movement;

public class MovingText : UpdateableText
{
    [SerializeField] private CharacterEngine _characterEngine;

    protected override void UpdateText()
    {
        _text.SetText(_characterEngine.IsMoving() ? "Moving" : "Stopped");
    }
}