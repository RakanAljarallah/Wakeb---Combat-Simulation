using System;
using Helper;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    private WeapenManager weapenManager;
    
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    public float damage = 10f;
    
    public Image weapenIcon;
    
   [SerializeField] private Sprite[] weapenIcons;
    
    private Animator zoomFPSAnimator;

    private bool zoomed;

    private Camera mainCamera;
    
    private GameObject crosshair;
    private void Awake()
    {
        weapenManager = GetComponent<WeapenManager>();
        zoomFPSAnimator = GameObject.FindGameObjectWithTag(Tags.FPS_CAMERA_TAG).gameObject.GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR_TAG);
        mainCamera = Camera.main;
    }

    public void ChangeFireType()
    {
        switch (weapenManager.currentWeapen.fireType)
        {
            case WeapenFireType.Single:
            {
                weapenManager.changeFireType(WeapenFireType.SemiAuto);
                weapenIcon.sprite = weapenIcons[1];
                break;
            }
            case WeapenFireType.SemiAuto:
            {
                weapenManager.changeFireType(WeapenFireType.Auto);
                weapenIcon.sprite = weapenIcons[2];
                break;
            }
            case WeapenFireType.Auto:
            {
                weapenManager.changeFireType(WeapenFireType.Single);
                weapenIcon.sprite = weapenIcons[0];
                break;
            }
        }
    }


    public void WeapenShot()
    {
        switch (weapenManager.currentWeapen.fireType)
        {
            default:
            {
                fireRate = Attributes.AUTO_RATE;
                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f/ fireRate;
                    weapenManager.currentWeapen.ShootAnimation();
                    ShootTarget();
                }
                break;
            }
            case WeapenFireType.Single:
            {
                fireRate = Attributes.SINGLE_RATE;
                
                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f/ fireRate;
                    ShootTarget();
                    weapenManager.currentWeapen.ShootAnimation();

                }
                break;
            }
            case WeapenFireType.SemiAuto:
            {
                fireRate = Attributes.SEMI_AUTO_RATE;
                if (Time.time > nextTimeToFire)
                {
                    ShootTarget();
                    nextTimeToFire = Time.time + 1f/ fireRate;
                    weapenManager.currentWeapen.ShootAnimation();
                }
                break;
            }
        }
        
        
    }
    
   public void ZoomInAndOut()
    {
        zoomed = !zoomed;
        zoomFPSAnimator.SetBool(AnimationTags.ZOOM_PARAMETER, zoomed);
        crosshair.SetActive(!zoomed);
    }
   
    
    public void ShootTarget()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 100))
        {
            if (hit.transform.TryGetComponent(out Target target))
            {
                
                target.TakeDamage(damage);
            }
           
        }
    }
}
