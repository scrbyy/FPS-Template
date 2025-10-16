using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputProvider : InputProvider
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private InputAction sprintAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }
    public override bool isJumpButtonDown()
    {
        return jumpAction.triggered; 
    }

    public override Vector2 GetKeyboardInput()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public override Vector2 GetMouseInput()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public override bool isInteractButtonDown()
    {
        return interactAction.triggered;
    }
    public override bool isSprintButtonPressed()
    {
        return sprintAction.IsPressed();
    }
}
