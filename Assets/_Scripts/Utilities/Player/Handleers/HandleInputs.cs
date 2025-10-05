using UnityEngine;
using UnityEngine.InputSystem;

public class HandleInputs : MonoBehaviour
{
    private Vector2 move, look;
    private float isAttacking, isRunning, isJumping, isInteracting;
    private float isDroping;
    [SerializeField] private PlayerInput playerInput;

    public void OnMove(InputAction.CallbackContext context) // Catch player input
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context) // Catch mouse input
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context) // Catch attack input
    {
        isAttacking = context.ReadValue<float>();
    }
    
    public void OnTryInteract(InputAction.CallbackContext context) // Catch attack input
    {
        isInteracting = context.ReadValue<float>();
    }
    
    public void OnTryDrop(InputAction.CallbackContext context) // Catch attack input
    {
        isDroping = context.ReadValue<float>();
    }

    public void OnRunning(InputAction.CallbackContext context) // Catch run input
    {
        isRunning = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context) // Catch run input
    {
        isJumping = context.ReadValue<float>();
    }

    public Vector2 GetMoveVector2() { return move; }  // Return public values

    public Vector2 GetLookVector2() { return look; }

    public bool IsAttacking() { return isAttacking == 1f; }

    public bool IsInteracting() { return isInteracting == 1f; }
    public bool DropInput() { return isDroping == 1f; }

    public bool IsRunning() { return isRunning == 1f; }

    public bool IsJumping() { return isJumping == 1f; }

    public void SetPaused(bool paused)
    {
        if (paused)
            playerInput.DeactivateInput();
        else
            playerInput.ActivateInput();
    }
}
