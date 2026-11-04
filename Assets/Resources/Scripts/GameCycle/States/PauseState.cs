using UnityEngine;

public class PauseState : IGameState
{
    public void Enter()
    {
        CursorVisibility.Show();
        Time.timeScale = 0f;
    }
    public void Exit() { }
}