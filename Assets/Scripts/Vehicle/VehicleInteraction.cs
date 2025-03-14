using System;
using Helper;
using Player;
using UnityEngine;

namespace Vehicle
{
    public class VehicleInteraction : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.PLAYER_TAG))
            {

               VehicaleManager.Instance.PlayerEnterVehicleRange();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(Tags.PLAYER_TAG))
            {
                VehicaleManager.Instance.PlayerExitVehicleRange();
            }
        }
    }
}
