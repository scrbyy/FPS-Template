using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestarButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();    
    }

    private void OnMouseDown()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnMouseDown);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnMouseDown);
    }
}
