using scrbyy.FpsTemplate.InputSystem;
using scrbyy.FpsTemplate.InputSystem.Interfaces;
using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class Pauser : MonoBehaviour
    {
        [Inject] private GameFSM _gameFSM;
        [Inject] IUIMenuProvider _uiMenuProvider;
        [Inject] private ActionMapChanger _actionMapChanger;

        public void Pause()
        {
            _gameFSM.SetState<PauseState>();
            _actionMapChanger.SetUIActionMap();
        }

        public void Unpause()
        {
            _gameFSM.SetState<GameplayState>();
            _actionMapChanger.SetPlayerActionMap();
        }

        private void OnEnable()
        {
            _uiMenuProvider.OnPauseMenuToggled += Handle;
        }
        private void OnDisable()
        {
            _uiMenuProvider.OnPauseMenuToggled -= Handle;
        }

        private void Handle()
        {
            if (_gameFSM.CurrentState is PauseState)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
}
