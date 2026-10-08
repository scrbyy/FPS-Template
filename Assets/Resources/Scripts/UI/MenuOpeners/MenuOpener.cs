using UnityEngine;
using Zenject;

public abstract class MenuOpener : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    [SerializeField] protected GameObject _hud;
    [Inject] private GameFSM _gameFSM;

    protected virtual void OpenMenu()
    {
        _menu.SetActive(true);
        _hud.SetActive(false);
        _gameFSM.SetState<PauseState>();
    }

    protected virtual void CloseMenu()
    {
        _menu.SetActive(false);
        _hud.SetActive(true);
        _gameFSM.SetState<GameplayState>();
    }
}