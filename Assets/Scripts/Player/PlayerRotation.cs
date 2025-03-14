using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] CinemachineInputAxisController inputAxisController;
    
    
    
    
    // Update is called once per frame
   public void ControlPlayerRotation(Transform camera)
    {
        transform.rotation = camera.rotation;
    }

    public void CheckCursorState()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            inputAxisController.enabled = true;
        }
        else
        {
            inputAxisController.enabled = false;
        }
    }
   
    
    
    
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(hasFocus);
        inputAxisController.enabled = hasFocus;
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
