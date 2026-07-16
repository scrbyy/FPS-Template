using UnityEngine;
using Zenject;

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

    public bool IsCollisionedBellow()
    {
        return _characterController.collisionFlags == CollisionFlags.Below;
    }

    public bool IsCollisionedAbove()
    {
        return _characterController.collisionFlags == CollisionFlags.Above;
    }
}