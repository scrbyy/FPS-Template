using UnityEngine;
using TMPro;
public class TargetSpeedText : MonoBehaviour 
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CharacterSpeed _characterSpeedHandler;

    private void Update()
    {
        _text.text = "Target Speed: " + _characterSpeedHandler.Speed.ToString();
    }
}