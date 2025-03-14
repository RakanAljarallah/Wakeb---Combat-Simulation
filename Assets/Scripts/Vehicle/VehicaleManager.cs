using Animancer;
using UnityEngine;

namespace Player
{
    public class VehicaleManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnplayerVehicleInteract;
        [SerializeField]
        private UnityEvent OnplayerVehicleExit;
        [SerializeField]
        private UnityEvent OnplayerVehicleEnter;
        [SerializeField]
        private UnityEvent OnplayerVehicleInteractExit;
        
        public static VehicaleManager Instance;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void InteractPlayerVehicleEnter()
        {
            OnplayerVehicleInteract.Invoke();
        }

        public void InteractPlayerVehicleExit()
        {
            OnplayerVehicleInteractExit.Invoke();
        }
        
        

        public void PlayerExitVehicleRange()
        {
            OnplayerVehicleExit.Invoke();
        }

        public void PlayerEnterVehicleRange()
        {
            OnplayerVehicleEnter.Invoke();
        }
    }
}