using UnityEngine;

public class TargetSpeedText : UpdateableText 
{
    [SerializeField] private CharacterSpeed _characterSpeedHandler;

    protected override void UpdateText()
    {
        _text.SetText("Target Speed: {0}", _characterSpeedHandler.Speed);
    }
}