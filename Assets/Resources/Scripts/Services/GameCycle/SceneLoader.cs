using UnityEngine.SceneManagement;

namespace scrbyy.FpsTemplate.Services.GameCycle
{
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
}