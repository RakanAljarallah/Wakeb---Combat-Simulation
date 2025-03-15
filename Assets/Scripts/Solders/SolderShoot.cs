using Helper;
using UnityEngine;

namespace Solders
{
    public class SolderShoot : MonoBehaviour
    {
        
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject muzzleFlash;
        
        [SerializeField] private Transform shotEffect;
        
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

        public RaycastHit ShotPoint()
        {
            // RaycastHit hit;

            if (Physics.Raycast(shotPoint.position, shotPoint.forward, out RaycastHit hit, 30f))
            {
                shotEffect.position = hit.point;
                shotEffect.GetComponent<ParticleSystem>().Play();
                return hit;
            }
            
            return hit;
        }
        
    }
}