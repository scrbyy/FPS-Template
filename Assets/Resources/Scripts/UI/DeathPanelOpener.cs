using UnityEngine;

public class DeathPanelOpener : MonoBehaviour
{
    [SerializeField] private CharacterHealth _playerHealth;
    [SerializeField] private GameObject _deathPanelUI;

    private void OnEnable()
    {
        _playerHealth.OnValueExhausted += ShowDeathPanel;
    }

    private void OnDisable()
    {
        _playerHealth.OnValueExhausted -= ShowDeathPanel;
    }

    private void ShowDeathPanel()
    {
        _deathPanelUI.SetActive(true);
        CursorVisibility.Show();
    }
}