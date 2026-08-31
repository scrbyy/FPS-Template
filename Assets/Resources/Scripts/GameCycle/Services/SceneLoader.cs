using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void RestartCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}