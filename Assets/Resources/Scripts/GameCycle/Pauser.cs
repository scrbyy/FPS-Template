using scrbyy.FpsTemplate.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class Pauser : MonoBehaviour
    {
        [Inject] private GameFSM _gameFSM;
        [Inject] IUIMenuProvider _uiMenuProvider;

        public void Pause()
        {
            _gameFSM.SetState<PauseState>();
        }

        public void Unpause()
        {
            _gameFSM.SetState<GameplayState>();
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
