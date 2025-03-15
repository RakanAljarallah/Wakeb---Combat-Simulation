using UnityEngine;
using UnityEngine.InputSystem;

public class InputFeedBack : MonoBehaviour
{
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isInteracting = false;
    public bool isSprinting = false;
    public Vector2 moveDirection;
    public Vector2 lookDirection;
    
    public bool cursorInputForLook = true;

    public bool isChangingFireType = false;
    
    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }
    
    
    public void OnJump(InputValue value)
    {
        isJumping = value.isPressed;
    }
    
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }
    
    public void OnAttack(InputValue value)
    {
        isAttacking = value.isPressed;
    }
    
    public void OnInteract(InputValue value)
    {
        isInteracting = value.isPressed;
    }

    public void OnChangeFireType(InputValue value)
    {
        isChangingFireType = value.isPressed;
    }
    
    public void OnEscape(InputValue value)
    {
        Cursor.lockState = value.isPressed ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
