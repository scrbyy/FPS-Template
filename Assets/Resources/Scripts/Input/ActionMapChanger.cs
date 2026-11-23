using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace scrbyy.FpsTemplate.InputSystem
{
    public class ActionMapChanger : MonoBehaviour
    {
        [Inject] private InputActionsAsset _inputActions;
        private InputActionMap _activeMap;

        public void SetPlayerActionMap()
        {
            ChangeActionMap(_inputActions.Player);
        }

        public void SetUIActionMap()
        {
            ChangeActionMap(_inputActions.UI);
        }

        private void ChangeActionMap(InputActionMap actionMap)
        {
            foreach (var map in _inputActions.asset.actionMaps)
            {
                map.Disable();
            }
            actionMap.Enable();
            _activeMap = actionMap;
        }

        private void OnDestroy()
        {
            _activeMap?.Disable();
            _activeMap = null;
        }
    }
}
