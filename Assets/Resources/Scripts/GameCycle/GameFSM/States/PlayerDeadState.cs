using UnityEngine;
using Zenject;

public class PlayerDeadState : IState
{
    private InputMapSelector _inputMapSelector;

    [Inject]
    public PlayerDeadState(InputMapSelector inputMapSelector)
    {
        _inputMapSelector = inputMapSelector;
    }

    public void Enter()
    {
        _inputMapSelector.SetUI();
        Time.timeScale = 0f;
    }

    public void Exit() { }
}
