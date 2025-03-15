using System;
using Helper;
using UnityEngine;

public class SolderAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Run(bool run)
    {
        animator.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    public void Shoot(bool shoot)
    {
        animator.SetBool(AnimationTags.FIRE_PARAMETER, shoot);
    }
    
    
    
    
}
