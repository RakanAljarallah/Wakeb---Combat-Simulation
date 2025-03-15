using System.Collections;
using Helper;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;


public class Target : MonoBehaviour
{
    private enum TargetState { Chase, Attack, Die }
    
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float deathDelay = 3f;
    [SerializeField] private Image healthBar;

 
    [SerializeField] private Transform baseTarget;
    
    private NavMeshAgent agent;
    private Animator animator;
    private IObjectPool<Target> pool;
    public float currentHealth;
    private bool isDead;
    public bool IsDead { get{return isDead;} private set{ isDead = value; } }
        
       
    // Animation hashes
    private static readonly int WalkHash = Animator.StringToHash(AnimationTags.WALK_PARAMETER);
    private static readonly int AttackHash = Animator.StringToHash(AnimationTags.ATTACK_PARAMETER);
    private static readonly int DeadHash = Animator.StringToHash(AnimationTags.DEAD_PARAMETER);

    public void SetPool(IObjectPool<Target> targetPool) => pool = targetPool;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        if (!baseTarget)
            baseTarget = GameObject.FindGameObjectWithTag(Tags.BASE_TAG)?.transform;
    }

    public void Initialize()
    {
        agent.ResetPath();
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthUI();
        SetState(TargetState.Chase);
    }

    public void ResetState()
    {
        StopAllCoroutines();
        animator.Rebind();
        animator.Update(0f);
    }

    private void SetState(TargetState newState)
    {
        StopAllCoroutines();
        
        switch (newState)
        {
            case TargetState.Chase:
                StartCoroutine(ChaseRoutine());
                break;
            case TargetState.Attack:
                StartCoroutine(AttackRoutine());
                break;
            case TargetState.Die:
                StartCoroutine(DieRoutine());
                break;
        }
    }

    private IEnumerator ChaseRoutine()
    {
        agent.isStopped = false;
        animator.SetBool(WalkHash, true);
        
        while (Vector3.Distance(transform.position, baseTarget.position) > attackDistance)
        {
            agent.SetDestination(baseTarget.position);
            yield return null;
        }
        
        SetState(TargetState.Attack);
    }

    private IEnumerator AttackRoutine()
    {
        agent.isStopped = true;
        animator.SetBool(WalkHash, false);
        
        var attackWait = new WaitForSeconds(attackCooldown);
        
        while (true)
        {
            animator.SetTrigger(AttackHash);

            yield return attackWait;
        }
    }

    private IEnumerator DieRoutine()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetBool(WalkHash, false);
        animator.SetBool(AttackHash, false);
        animator.SetBool(DeadHash, true);
        
        yield return new WaitForSeconds(deathDelay);
        pool.Release(this);
    }

    public bool TakeDamage(float damage)
    {
        if (IsDead) return false;
    
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            IsDead = true;
            SetState(TargetState.Die);
            return true;
        }
        return false;
    }

    private void UpdateHealthUI()
    {
        if (healthBar)
            healthBar.fillAmount = currentHealth / maxHealth;
    }

    
}