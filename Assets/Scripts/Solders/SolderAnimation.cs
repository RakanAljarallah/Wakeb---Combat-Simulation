using Helper;
using UnityEngine;

public class SolderAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isRunning;
    private bool isShooting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Run(bool run)
    {
        if(isRunning == run) return;
        isRunning = run;
        animator.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    public void Shoot(bool shoot)
    {
        if(isShooting == shoot) return;
        isShooting = shoot;
        animator.SetBool(AnimationTags.FIRE_PARAMETER, shoot);
    }
}