using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class GroundedStateText : MonoBehaviour
{
    private TMP_Text _text;

    [Inject] private IGroundChecker _groundChecker;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = $"Grounded: {_groundChecker.IsGrounded} ";
    }
}