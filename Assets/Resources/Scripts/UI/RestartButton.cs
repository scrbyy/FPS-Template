using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    private void OnMouseDown()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnMouseDown);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnMouseDown);
    }
}
