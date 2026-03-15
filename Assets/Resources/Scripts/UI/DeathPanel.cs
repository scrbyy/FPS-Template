using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject deathPanelUI;

    private void OnEnable()
    {
        playerHealth.OnValueExhausted += ShowDeathPanel;
    }

    private void OnDisable()
    {
        playerHealth.OnValueExhausted -= ShowDeathPanel;
    }

    private void ShowDeathPanel()
    {
        deathPanelUI.SetActive(true);
        CursorVisibility.Show();
    }
}
