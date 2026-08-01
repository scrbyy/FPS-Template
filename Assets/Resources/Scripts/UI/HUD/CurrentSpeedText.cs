using TMPro;
using UnityEngine;

public class CurrentSpeedText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CharacterEngine _characterEngine;

    private void Update()
    {
        _text.text = "Curent Velocity: " + _characterEngine.Velocity.ToString();
    }
}