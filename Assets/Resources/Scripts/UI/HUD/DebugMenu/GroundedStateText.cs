using TMPro;
using UnityEngine;
using Zenject;

using scrbyy.FpsTemplate.Characters.Player.Movement.GroundChecker;

namespace scrbyy.FpsTemplate.UI.HUD.DebugMenu
{
    [RequireComponent(typeof(TMP_Text))]
    public class GroundedStateText : UpdateableText
    {
        [Inject] private IGroundChecker _groundChecker;

        protected override void UpdateText()
        {
            _text.SetText(_groundChecker.IsGrounded ? "Grounded" : "Not Grounded");
        }
    }
}