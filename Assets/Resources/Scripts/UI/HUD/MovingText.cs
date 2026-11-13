using UnityEngine;

using scrbyy.FpsTemplate.Player.Movement;

public class MovingText : UpdateableText
{
    [SerializeField] private CharacterEngine _characterEngine;

    protected override void UpdateText()
    {
        _text.SetText(_characterEngine.IsMoving() ? "Moving" : "Stopped");
    }
}