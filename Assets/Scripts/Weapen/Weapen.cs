using System;
using Helper;
using UnityEngine;


public enum WeapenFireType
{
    Single,
    SemiAuto,
    Auto
}
public class Weapen : MonoBehaviour
{
    public int ammoCapacity;
    public int currentAmmo;

    public float damage;
    public float fireRate;
    public float shotForece = 10f;
    
    private Animator animator;
    // public WeapenAime aime;

    [SerializeField] private GameObject shotFlash;
    
    [SerializeField]
    private AudioSource shotSound, reloadSound;
    
    public WeapenFireType fireType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void ShootAnimation()
    {
        animator.SetTrigger(AnimationTags.FIRE_PARAMETER);
        
    }
    
    
  public  void ActivateShotFlashON()
    {
        shotFlash.SetActive(true);
    }
    
  public  void ActivateShotFlashOFF()
    {
        shotFlash.SetActive(false);
    }

   public void playShotSound()
    {
        shotSound.Play();
    }

    void ReloadSound()
    {
        reloadSound.Play();
    }
    
}
