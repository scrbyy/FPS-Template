using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        CursorVisibility.Hide();
    }  
}