using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class GroundedStateText : UpdateableText
{
    [Inject] private GroundChecker _groundChecker;

    protected override void UpdateText()
    {
        _text.SetText(_groundChecker.IsGrounded ? "Grounded" : "Not Grounded");
    }
}