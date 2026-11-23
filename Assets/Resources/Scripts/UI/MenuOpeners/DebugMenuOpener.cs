using scrbyy.FpsTemplate.InputSystem.Interfaces;
using Zenject;

namespace scrbyy.FpsTemplate.UI.MenuOpeners
{
    public class DebugMenuOpener : MenuOpener
    {
        [Inject] private IUIMenuProvider _uiMenuProvider;

        protected override void Unsubscribe()
        {
            _uiMenuProvider.OnDebugMenuToggled -= ToggleMenu;
        }

        protected override void Subscribe()
        {
            _uiMenuProvider.OnDebugMenuToggled += ToggleMenu;
        }
    }
}
