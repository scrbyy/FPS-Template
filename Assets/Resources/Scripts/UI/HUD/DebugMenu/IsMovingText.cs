using UnityEngine;

using scrbyy.FpsTemplate.Player.Movement;

namespace scrbyy.FpsTemplate.UI.HUD.DebugMenu
{
    public class IsMovingText : UpdateableText
    {
        [SerializeField] private CharacterEngine _characterEngine;

        protected override void UpdateText()
        {
            _text.SetText(_characterEngine.IsMoving() ? "Moving" : "Stopped");
        }
    }
}