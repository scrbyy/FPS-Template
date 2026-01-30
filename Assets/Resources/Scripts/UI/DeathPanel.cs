using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject deathPanelUI;

    private void OnEnable()
    {
        playerHealth.OnPlayerDeath += ShowDeathPanel;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerDeath -= ShowDeathPanel;
    }

    private void ShowDeathPanel()
    {
        deathPanelUI.SetActive(true);
        CursorVisibility.Show();
    }
}
