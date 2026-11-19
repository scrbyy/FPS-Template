using UnityEngine;

using scrbyy.FpsTemplate.Services.UI;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class PauseState : IGameState
    {
        public void Enter()
        {
            CursorVisibility.Show();
            Time.timeScale = 0f;
        }
        public void Exit() { }
    }
}