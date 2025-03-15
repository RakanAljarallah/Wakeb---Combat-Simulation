using UnityEngine;
using UnityEngine.Pool;

namespace Targets
{
    public class TargetSpawner : MonoBehaviour
    {
       
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private float timeBetweenSpawn = 5f;
        [SerializeField] private int defaultPoolSize = 10;
        [SerializeField] private int maxPoolSize = 20;
        
        [SerializeField] private Target targetPrefab;

        private IObjectPool<Target> targetPool;
        private float nextSpawnTime;

        private void Awake()
        {
            targetPool = new ObjectPool<Target>(
                CreateTarget,
                OnGetFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                true,
                defaultPoolSize,
                maxPoolSize
            );
        }

        private void Update()
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnTarget();
                nextSpawnTime = Time.time + timeBetweenSpawn;
            }
        }

        private void SpawnTarget()
        {
            if (spawnPoints.Length == 0) return;
            
            targetPool.Get();
        }

        private Target CreateTarget()
        {
            Target target = Instantiate(targetPrefab);
            target.SetPool(targetPool);
            return target;
        }

        private void OnGetFromPool(Target target)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            target.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            target.gameObject.SetActive(true);
            target.Initialize();
        }

        private void OnReturnToPool(Target target)
        {
            target.gameObject.SetActive(false);
            target.ResetState();
        }

        private void OnDestroyPoolObject(Target target)
        {
            Destroy(target.gameObject);
        }
    }
}