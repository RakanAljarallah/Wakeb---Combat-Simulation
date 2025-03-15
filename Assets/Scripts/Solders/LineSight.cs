using UnityEngine;

public class LineSight : MonoBehaviour
{
    
    [SerializeField] private float visionRange = 15f;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private LayerMask targetMask;
    
    
    [HideInInspector] public bool canSeeTarget = false;
    [HideInInspector] public Vector3 lastKnownPosition = Vector3.zero;
   [HideInInspector] public Transform target = null;

    private Collider[] potentialTargets = new Collider[5];
    private float checkInterval = 0.25f;
    private float checkTimer;

    private void Awake()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        if(collider) collider.radius = visionRange;
    }

    private void Update()
    {
        checkTimer += Time.deltaTime;
        if(checkTimer >= checkInterval)
        {
            CheckForTargets();
            checkTimer = 0;
        }
    }

    private void CheckForTargets()
    {
        canSeeTarget = false;
        
        int targetsFound = Physics.OverlapSphereNonAlloc(
            transform.position,
            visionRange,
            potentialTargets,
            targetMask
        );

        if (targetsFound > 0)
        {
            for (int i = 0; i < potentialTargets.Length; i++)
            {
                if (potentialTargets[i] == null) continue;
                Target potentilaTarget;
                
                if (potentialTargets[i].gameObject.TryGetComponent<Target>(out potentilaTarget))
                {
                    canSeeTarget = true;
                    target = potentilaTarget.transform;
                    lastKnownPosition = potentilaTarget.transform.position;
                }
            }
        }
    }
    
    
}