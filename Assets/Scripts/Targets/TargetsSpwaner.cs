
using UnityEngine;
using UnityEngine.Pool;
namespace Targets
{
    public class TargetsSpwaner : MonoBehaviour
    {
        [SerializeField] private Transform[] spwanPoints;
        [SerializeField] private float timeBetweenSpwan = 5f;

        private float timeSinceLastSpwan;
        
        [SerializeField] private Target targetPrefab;
        private IObjectPool<Target> targetPool;

        private void Awake()
        {
            targetPool = new ObjectPool<Target>(CreateTarget, OnGet, OnRelease);
        }
        
        private void OnGet(Target target)
        {
            target.transform.position = spwanPoints[Random.Range(0, spwanPoints.Length)].position;
            target.gameObject.SetActive(true);
        }

        private void OnRelease(Target target)
        {
            target.gameObject.SetActive(false);
        }
        
        private Target CreateTarget()
        {
            Target target =  Instantiate(targetPrefab);
            target.SetPool(targetPool);
            return target;
        }

        private void Update()
        {
            if (Time.time > timeSinceLastSpwan)
            {
                targetPool.Get();
                timeSinceLastSpwan = Time.time + timeBetweenSpwan;
                
            }
        }
    }
}