using scrbyy.FpsTemplate.Characters.Stats;
using UnityEngine;

namespace scrbyy.FpsTemplate.UI.MenuOpeners
{
    public class DeathMenuOpener : MenuOpener
    {
        [SerializeField] private CharacterHealth _playerHealth;

        private void OnEnable()
        {
            _playerHealth.OnValueExhausted += OpenMenu;
        }

        private void OnDisable()
        {
            _playerHealth.OnValueExhausted -= OpenMenu;
        }
    }
}