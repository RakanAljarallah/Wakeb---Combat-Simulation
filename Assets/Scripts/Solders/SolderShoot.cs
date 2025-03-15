using Helper;
using UnityEngine;

namespace Solders
{
    public class SolderShoot : MonoBehaviour
    {
        
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject muzzleFlash;
        
        private void Start()
        {
            muzzleFlash.SetActive(false);
        }
        
        public void TunrOnMuzzleFlash()
        {
            muzzleFlash.SetActive(true);
        }
        
        public void TunrOffMuzzleFlash()
        {
            muzzleFlash.SetActive(false);
        }

        public Transform ShotPoint()
        {
            RaycastHit hit;

            if (Physics.Raycast(shotPoint.position, shotPoint.forward, out hit, 30f))
            {
                return hit.transform;
                
            }

            return null;
        }
        
    }
}