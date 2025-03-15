using UnityEngine;
using UnityEngine.Pool;

namespace Solders
{
    public class SolderShoot : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private ParticleSystem shotEffect;
        
        [Header("Settings")]
        [SerializeField] private float shotRange = 30f;
        [SerializeField] private int shotDamage = 10;

        private RaycastHit[] raycastHits = new RaycastHit[1];
        private ObjectPool<GameObject> muzzleFlashPool;
        private bool muzzleActive;

        private void Start()
        {
            muzzleFlashPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(muzzleFlash),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                defaultCapacity: 5
            );
        }

        public void TunrOnMuzzleFlash()
        {
            if(muzzleActive) return;
            muzzleActive = true;
            muzzleFlashPool.Get();
        }
        
        public void TunrOffMuzzleFlash()
        {
            if(!muzzleActive) return;
            muzzleActive = false;
            muzzleFlashPool.Release(muzzleFlash);
        }

        public bool ShotPoint()
        {
            int hits = Physics.RaycastNonAlloc(
                shotPoint.position, 
                shotPoint.forward, 
                raycastHits,
                shotRange
            );

            if(hits > 0)
            {
                var hit = raycastHits[0];
                if(TryGetTarget(hit.transform, out Target target))
                {
                    bool targetDied = target.TakeDamage(shotDamage);
                    PlayShotEffect(hit.point);
                    return targetDied;
                }
            }
            return false;
        }

        private bool TryGetTarget(Transform t, out Target target)
        {
            if(t.TryGetComponent<Target>(out target))
            {
                return !target.IsDead; 
            }
            return false;
        }

        private void PlayShotEffect(Vector3 position)
        {
            shotEffect.transform.position = position;
            if(!shotEffect.isPlaying)
            {
                shotEffect.Play(true);
            }
        }
    }
}