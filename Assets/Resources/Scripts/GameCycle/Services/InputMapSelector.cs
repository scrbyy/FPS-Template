using Zenject;

public class InputMapSelector
{
    private InputSettings _inputSettings;

    [Inject] 
    public InputMapSelector(InputSettings settings)
    {
        _inputSettings = settings;
        _inputSettings.Enable();
    }
    public void SetUI()
    {
        _inputSettings.Disable();
        _inputSettings.UI.Enable();
    }

    public void SetGameplay()
    {
        _inputSettings.Disable();
        _inputSettings.Player.Enable();
    }
}