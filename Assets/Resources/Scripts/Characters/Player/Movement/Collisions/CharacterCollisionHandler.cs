using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.Characters.Player.Movement
{
    public class CharacterCollisionHandler
    {
        private CharacterController _characterController;

        [Inject]
        public CharacterCollisionHandler(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public bool IsCollisionedBySide()
        {
            return (_characterController.collisionFlags & CollisionFlags.Sides) != 0;
        }

        public bool IsCollisionedAbove()
        {
            return (_characterController.collisionFlags & CollisionFlags.Above) != 0;
        }
    }
}