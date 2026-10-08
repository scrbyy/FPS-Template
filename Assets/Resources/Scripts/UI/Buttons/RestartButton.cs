using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [Inject] private SceneLoader _sceneLoader;

    private void RestartScene()
    {
        _sceneLoader.RestartCurrent();
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
