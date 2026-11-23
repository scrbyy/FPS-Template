using UnityEngine;

using scrbyy.FpsTemplate.Services.UI;
using Zenject;
using scrbyy.FpsTemplate.InputSystem;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class GameplayState : IGameState
    {
        [Inject] private ActionMapChanger _actionMapChanger;

        public void Enter()
        {
            CursorVisibility.Hide();
            Time.timeScale = 1.0f;
            _actionMapChanger.SetPlayerActionMap();
        }

        public void Exit() { }
    }
}