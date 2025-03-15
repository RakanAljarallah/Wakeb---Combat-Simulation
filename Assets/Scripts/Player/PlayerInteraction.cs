using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
    
        public void RideVehicle( Transform vehicle )
        {
            transform.SetParent(vehicle);
            
            gameObject.SetActive(false);
        }

        public void ExitedVehicle(Transform parent = null)
        {
            transform.SetParent(parent);
       
            gameObject.SetActive(true);
        }
        
    }
}


