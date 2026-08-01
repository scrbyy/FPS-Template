using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]

public class MovingText : MonoBehaviour
{
    [SerializeField] private CharacterEngine _characterEngine;

    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = "Is Moving: " + _characterEngine.IsMoving();
    }
}