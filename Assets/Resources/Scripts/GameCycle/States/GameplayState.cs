using UnityEngine;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class GameplayState : IGameState
    {
        public void Enter()
        {
            CursorVisibility.Hide();
            Time.timeScale = 1.0f;
        }
        public void Exit() { }
    }
}