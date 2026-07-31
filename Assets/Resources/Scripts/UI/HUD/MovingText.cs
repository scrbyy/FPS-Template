using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]

public class MovingText : MonoBehaviour
{
    private TMP_Text _text;
    [SerializeField] private CharacterEngine _characterEngine;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        _text.text = "Is Moving: " + _characterEngine.IsMoving();
    }
}
