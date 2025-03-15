using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Solders
{
    public class SolderAI : MonoBehaviour
    {
        public enum EnemyState { Patrolling, Chasing, Attacking }
        
        public float attackRange = 10f;
        public float movementSpeed = 5f;
        public Transform patrolPoint; 
        
        [SerializeField] private LineSight lineSight;
        
         
        private NavMeshAgent agent;
        private SolderAnimation solderAnimation;
        private SolderSound solderSound;
        private SolderShoot solderShoot;
        
         
        private EnemyState currentState;
        private Transform currentTarget;
        private WaitForSeconds stateUpdateDelay = new WaitForSeconds(0.2f);

        private EnemyState CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                
                StopAllCoroutines();
                switch(currentState)
                {
                    case EnemyState.Patrolling:
                         
                        StartCoroutine(PatrolRoutine());
                        break;
                    
                    case EnemyState.Chasing:
                         
                        StartCoroutine(ChaseRoutine());
                        break;
                    
                    case EnemyState.Attacking:
                       
                        StartCoroutine(AttackRoutine());
                        break;
                }
            }
        } 
     
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            solderAnimation = GetComponent<SolderAnimation>();
            solderSound = GetComponent<SolderSound>();
            solderShoot = GetComponent<SolderShoot>();
            
            agent.speed = movementSpeed;
            agent.stoppingDistance = 1f; 
        }

        private void Start()
        {
            currentTarget = patrolPoint;

            CurrentState = EnemyState.Patrolling;

        }

        

        

        private IEnumerator PatrolRoutine()
        {
            agent.stoppingDistance = 1f;  
    
            while(currentState == EnemyState.Patrolling)
            {
                 
                agent.SetDestination(patrolPoint.position);
                agent.isStopped = false;

                 
                solderAnimation.Run(agent.velocity.magnitude > 0.1f);
                solderSound.PlayRunSound(agent.velocity.magnitude > 0.1f);

                 
                if(lineSight.canSeeTarget)
                {
                    currentTarget = lineSight.target;
                    CurrentState = EnemyState.Chasing;
                    yield break;
                }

                 
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator ChaseRoutine()
        {
            agent.stoppingDistance = attackRange * 0.9f;  
    
            while(currentState == EnemyState.Chasing)
            {
                if(currentTarget == null)  
                {
                    CurrentState = EnemyState.Patrolling;
                    yield break;
                }

                agent.SetDestination(currentTarget.position);
                solderAnimation.Run(agent.velocity.magnitude > 0.1f);
                solderSound.PlayRunSound(agent.velocity.magnitude > 0.1f);
        
                 
                float distance = Vector3.Distance(transform.position, currentTarget.position);
                if(distance <= attackRange)
                {
                    CurrentState = EnemyState.Attacking;
                    yield break;
                }
                
                FaceTarget();
                
                yield return null;
            }
        }
         

        private IEnumerator AttackRoutine()
        {
            var attackDelay = new WaitForSeconds(0.5f);
            solderAnimation.Run(false);
            while(currentState == EnemyState.Attacking)
            {
                solderShoot.TunrOffMuzzleFlash();
                if(currentTarget == null || 
                   Vector3.Distance(transform.position, currentTarget.position) > attackRange * 1.1f)
                {
                    CurrentState = EnemyState.Chasing;
                    yield break;
                }

                FaceTarget();
                solderShoot.ShotPoint();
                solderSound.PlayShotSound();
                solderShoot.TunrOnMuzzleFlash();
                yield return attackDelay;
                
            }
            
            agent.isStopped = false;
        }

        private void FaceTarget()
        {
            if(currentTarget == null) return;
            
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                lookRotation, 
                Time.deltaTime * 10f
            );
        }

        
    }
}