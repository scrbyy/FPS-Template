using UnityEngine;

namespace scrbyy.FpsTemplate.UI.MenuOpeners
{
    public abstract class MenuOpener : MonoBehaviour
    {
        [SerializeField] protected GameObject _menu;

        protected virtual void OpenMenu()
        {
            if (_menu.activeSelf == true) return;
            _menu.SetActive(true);
        }

        protected virtual void CloseMenu()
        {
            if (_menu.activeSelf == false) return;
            _menu.SetActive(false);
        }

        protected virtual void ToggleMenu()
        {
            if(_menu.activeSelf == true)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        private void OnEnable() => Subscribe();
        private void OnDisable() => Unsubscribe();
    }
}