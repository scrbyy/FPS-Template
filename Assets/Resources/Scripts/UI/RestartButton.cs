using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    private void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(RestartScene);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(RestartScene);
    }
}
