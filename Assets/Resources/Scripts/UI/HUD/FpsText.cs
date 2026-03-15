using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FpsText : MonoBehaviour
{
    private TMP_Text _fpsText;

    private void Start() => _fpsText = GetComponent<TMP_Text>();

    private void Update() => _fpsText.text = (Mathf.Round(1f / Time.deltaTime)) + " FPS";
}
