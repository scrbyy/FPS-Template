using scrbyy.FpsTemplate.Characters.Stats;
using UnityEngine;

namespace scrbyy.FpsTemplate.UI.MenuOpeners
{
    public class DeathMenuOpener : PauseMenuOpener
    {
        [SerializeField] private CharacterHealth _playerHealth;

        protected override void Subscribe()
        {
            _playerHealth.OnValueExhausted += OpenMenu;
        }

        protected override void Unsubscribe()
        {
            _playerHealth.OnValueExhausted -= OpenMenu;
        }
    }
}