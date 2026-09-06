using UnityEngine;
using Zenject;

public class PlayState : IState
{
    private InputMapSelector _inputMapSelector;

    [Inject]
    public PlayState(InputMapSelector inputMapSelector)
    {
        _inputMapSelector = inputMapSelector;
    }

    public void Enter()
    {
        _inputMapSelector.SetGameplay();
        Time.timeScale = 1.0f;
    }

    public void Exit() { }
}