using UnityEngine;

using scrbyy.FpsTemplate.Player.Movement;

namespace scrbyy.FpsTemplate.UI.HUD.DebugMenu
{
    public class TargetSpeedText : UpdateableText
    {
        [SerializeField] private CharacterSpeed _characterSpeed;

        protected override void UpdateText()
        {
            _text.SetText("Target Speed: {0}", _characterSpeed.Speed);
        }
    }
}