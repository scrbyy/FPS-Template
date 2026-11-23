using UnityEngine;

using scrbyy.FpsTemplate.Services.UI;
using scrbyy.FpsTemplate.InputSystem;
using Zenject;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class PauseState : IGameState
    {
        [Inject] private ActionMapChanger _actionMapChanger;
        public void Enter()
        {
            CursorVisibility.Show();
            Time.timeScale = 0f;
            _actionMapChanger.SetUIActionMap();
        }
        public void Exit() { }
    }
}