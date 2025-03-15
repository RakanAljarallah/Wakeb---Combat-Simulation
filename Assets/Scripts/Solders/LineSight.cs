using System;
using Helper;
using UnityEngine;

public class LineSight : MonoBehaviour
{
   //public float fieldOfView = 90f;
    
    public Transform target = null;
    
    public Transform eyePoint = null;
    
    public bool canSeeTraget = false;

    private SphereCollider sphereCollider = null;
    
    public Vector3 lastKnownPosition = Vector3.zero;
    
    private string entityTag = Tags.TARGET_TAG;

    public bool isSolder = true;
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        lastKnownPosition = transform.position;
        
        entityTag = isSolder ? Tags.TARGET_TAG : Tags.PLAYER_TAG;
    }

    void OnTriggerStay(Collider other)
    {
      
        if (other.CompareTag(entityTag))
        {
            canSeeTraget = true;
            lastKnownPosition = other.transform.position;
            target = other.transform;
        }
        
    }

    // bool INFoV()
    // {
    //     Vector3 direToTarget = target.position - eyePoint.position;
    //     
    //     float angleToTarget = Vector3.Angle(eyePoint.forward, direToTarget);
    //     
    //     if(angleToTarget <= fieldOfView)
    //     {
    //         return true;
    //     }
    //     
    //
    //     return false;
    // }

    // bool isTargetInSight()
    // {
    //     RaycastHit hit;
    //     
    //     if(Physics.Raycast(eyePoint.position, target.position - eyePoint.position, out hit, sphereCollider.radius))
    //     {
    //         if(hit.transform.CompareTag(Tags.TARGET_TAG))
    //         {
    //             lastKnownPosition = target.position;
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }
}
