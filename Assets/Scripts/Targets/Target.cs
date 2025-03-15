using System;
using System.Collections;
using Helper;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform baseTarget;
    
    private float distanceToAttack = 2f;
    public float currentHealth = 100f;
    private float maxHealth = 100f;
    
    [SerializeField] Image healthBar;
    
    public enum TargetState {Chase, Attack, Die}
    private TargetState currentState = TargetState.Chase;
    
    private IObjectPool<Target> targetPool;
    public void SetPool(IObjectPool<Target> pool)
    {
        targetPool = pool;
    }
    
    public TargetState CurrState
    {
        get => currentState;
        set
        {
            currentState = value;
            StopAllCoroutines();
            
            switch (currentState)
            {
                case TargetState.Chase:
                {
                    StartCoroutine(Chase());
                    break;
                }
                case TargetState.Attack:
                {
                    StartCoroutine(Attack());
                    break;
                }
                case TargetState.Die:
                {
                    StartCoroutine(Die());
                    break;
                }
            }
        }
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        baseTarget = GameObject.FindGameObjectWithTag(Tags.BASE_TAG).transform;
    }

    private void Start()
    {
        CurrState = TargetState.Chase;
    }

    private void OnEnable()
    {
        CurrState = TargetState.Chase;
        healthBar.fillAmount = maxHealth / 100;
    }

    private IEnumerator Chase()
    {
        while (currentState == TargetState.Chase)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(baseTarget.position);
            animator.SetBool(AnimationTags.WALK_PARAMETER, true);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                navMeshAgent.isStopped = true;
                CurrState = TargetState.Attack;
            }
            yield return null;
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / 100;
        
        if (currentHealth <= 0)
        {
            CurrState = TargetState.Die;
        }
    }


    private IEnumerator Attack()
    {
        while (currentState == TargetState.Attack)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool(AnimationTags.ATTACK_PARAMETER, true);
            yield return new WaitForSeconds(1f);
            animator.SetBool(AnimationTags.ATTACK_PARAMETER, false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private IEnumerator Die()
    {
        while (currentState == TargetState.Die)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool(AnimationTags.DEAD_PARAMETER, true);
            yield return new WaitForSeconds(3f);
            targetPool.Release(this);
        }
    }
    
    
    
    
    
}
