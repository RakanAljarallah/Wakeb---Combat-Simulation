using Helper;
using Player;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
   [SerializeField] private Transform fpPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    { 
        
        
        Vector3 PlayerPos = fpPosition.position;
        // Vector3 worldPos = transform.TransformPoint(PlayerPos);
        // Vector3 LookDirection = (worldPos - transform.TransformPoint(transform.position)).normalized;
       // Debug.Log("looking at player location : " + PlayerPos);
      transform.LookAt(PlayerPos);
    }
}
