using Zenject;
using UnityEngine;
using scrbyy.FpsTemplate.GameCycle;

namespace scrbyy.FpsTemplate.UI.MenuOpeners
{
    public class PauseMenuOpener : MenuOpener
    {
        [SerializeField] protected GameObject _hud;
        [Inject] private GameFSM _gameFSM;

        protected override void OpenMenu()
        {
            base.OpenMenu();
            _hud.SetActive(false);
        }

        protected override void CloseMenu()
        {
            base.CloseMenu();
            _hud.SetActive(true);
        }

        protected override void Subscribe()
        {
            _gameFSM.OnStateChanged += Handle;
        }

        protected override void Unsubscribe()
        {
            _gameFSM.OnStateChanged -= Handle;
        }

        private void Handle(IGameState state)
        {
            if (state is PauseState)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }
}