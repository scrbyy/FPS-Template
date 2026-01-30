using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestarButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();    
        button.onClick.AddListener(OnMouseDown);
    }

    private void OnMouseDown()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnMouseDown);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnMouseDown);
    }
}
