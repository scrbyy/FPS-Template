using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputProvider : InputProvider
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction interactAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
    }
    public override bool isJumpButtonPressed()
    {
        return jumpAction.IsPressed(); 
    }

    public override Vector2 GetKeyboardInput()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public override Vector2 GetMouseInput()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public override bool isInteractButtonPressed()
    {
        return interactAction.triggered;
    }
}
