using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
    
        public void RideVehicle( Transform vehicle )
        {
            transform.SetParent(vehicle);
            // vehicle.gameObject.GetComponent<VehicleController>().enabled = true;
            // vehicle.Find("InteractionUI").gameObject.SetActive(false);
            Debug.Log("FPP set to false");
            gameObject.SetActive(false);
        }

        public void ExitedVehicle(Transform parent = null)
        {
            transform.SetParent(parent);
            Debug.Log("FPP set to true");
            gameObject.SetActive(true);
        }
        
    }
}


