using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Solders
{
    public class SolderAI : MonoBehaviour
    {
        public enum EnemeyState { Patrolling, Chasing, Attacking, TakingCover }
        
        private EnemeyState currentState = EnemeyState.Patrolling;
        
        
        private NavMeshAgent agent;
        public Transform PatrollingTarget;
       [HideInInspector] public Transform currentPatrollingTarget;
        
        public float attackRange = 10f;
        public float movementSpeed = 5f;
        
        LineSight lineSight;
        SolderAnimation solderAnimation;
        SolderSound solderSound;
        SolderShoot solderShoot;
        
        
        public EnemeyState CurrentState
        {
            get => currentState;
            set
            { 
                currentState = value;
                StopAllCoroutines();

                switch (currentState)
                {
                    case EnemeyState.Patrolling:
                    {
                        StartCoroutine(AIPatrolling());
                        break;
                    }
                    case EnemeyState.Chasing:
                    {
                        StartCoroutine(Chasing());
                        break;
                    }
                    case EnemeyState.Attacking:
                    {
                        StartCoroutine(Attacking());
                        break;
                    }
                }
            }
        }
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            lineSight = GetComponentInChildren<LineSight>();
            solderAnimation = GetComponent<SolderAnimation>();
            solderSound = GetComponent<SolderSound>();
            solderShoot = GetComponent<SolderShoot>();
        }

        private void Start()
        {
            CurrentState = EnemeyState.Patrolling;
            currentPatrollingTarget = PatrollingTarget;
        }

        public IEnumerator AIPatrolling()
        {
            while (currentState == EnemeyState.Patrolling)
            {
                agent.isStopped = false;
                agent.SetDestination(currentPatrollingTarget.position);

                while (agent.pathPending)
                {
                    yield return null;
                }
                if (lineSight.canSeeTraget)
                {
                    StopState();
                    agent.isStopped = true;
                    
                    CurrentState = EnemeyState.Chasing;
                    yield break;
                }
                
                RunState();

                yield return null;
            }
            yield return null;
        }
        
        public IEnumerator Chasing()
        {
            while (currentState == EnemeyState.Chasing)
            {
                
                agent.isStopped = false;
                agent.SetDestination(lineSight.lastKnownPosition);
                
                while (agent.pathPending)
                    yield return null;

                if (agent.remainingDistance <= attackRange)
                {
                    StopState();
                    agent.isStopped = true;

                    if (!lineSight.canSeeTraget)
                    {
                        CurrentState = EnemeyState.Patrolling;
                    }
                    else
                    {
                        CurrentState = EnemeyState.Attacking;
                    }
                    
                    yield break;
                }
                
                // implement chase logic here
                RunState();
                yield return null;
            }
            yield return null;
        }
        
        public IEnumerator Attacking()
        {
            while (currentState == EnemeyState.Attacking)
            {

                agent.isStopped = false;
                agent.SetDestination(lineSight.target.position);

                while (agent.pathPending)
                {
                    yield return null;
                }

                if (agent.remainingDistance > attackRange)
                {
                    StopShoot();
                    agent.isStopped = true;
                    CurrentState = EnemeyState.Chasing;
                    yield break;
                }
                
                StopState();
                ShootState();
                
                yield return null;
            }
            yield return null;
        }
        
        private void RunState()
        {
            solderAnimation.Run(true);
            solderSound.PlayRunSound();
        }

        private void ShootState()
        {
            solderAnimation.Shoot(true);
            solderSound.PlayShotSound();
            solderShoot.TunrOnMuzzleFlash();
        }

        private void StopShoot()
        {
            solderAnimation.Shoot(false);
            solderShoot.TunrOffMuzzleFlash();
        }

        private void StopState()
        {
            solderAnimation.Run(false);
        }
    
    }
}


