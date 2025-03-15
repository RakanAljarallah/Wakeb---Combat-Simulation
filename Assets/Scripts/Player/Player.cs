using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private InputFeedBack inputFeedBack;
        private PlayerMovement playerMovement;
        private PlayerRotation playerRotation;
        private PlayerFootSteps playerFootSteps;
        private PlayerShoot playerShoot;
        private PlayerInteraction playerInteraction;
        
        [Header("References")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Transform camera;
        [SerializeField] private Transform gunPoint;
        
        [Space(5)]
        [Header("steps volume")]
        private float sprint_volume = 1f;

        private float walk_volume_min = 0.2f, walk_volume_max = 0.6f;
	    
        private float walk_step_distance = 0.4f;
        private float sprint_step_distance = 0.25f;
       
        private void Awake()
        {
            inputFeedBack = GetComponent<InputFeedBack>();
            playerMovement = GetComponent<PlayerMovement>();
            playerRotation = GetComponent<PlayerRotation>();
            playerFootSteps = GetComponent<PlayerFootSteps>();
            playerShoot = GetComponent<PlayerShoot>();
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        
        private void Start()
        {
            playerFootSteps.volumeMin = walk_volume_min;
            playerFootSteps.volumeMax = walk_volume_max;
            playerFootSteps.stepsN = walk_step_distance;
        }
        private void Update()
        {
            Move();
            StepsSound();
            Look();
            Jump();
            Aim();
            Attack();
            Interact();
            CheckCursorState();
            ChangeFireType();
        }
        
        private void FixedUpdate()
        {
            playerMovement.ApplyGravity();

        }
    
        private void Aim()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _playerAnimator.speed = 0;
                playerShoot.ZoomInAndOut();
            }

            if(Input.GetMouseButtonUp(1))
            {
                _playerAnimator.speed = 1;
                playerShoot.ZoomInAndOut();
            }
        }

        private void CheckCursorState()
        {
            playerRotation.CheckCursorState();
        }

        private void Attack()
        {
            if (inputFeedBack.isAttacking)
            {
                playerShoot.WeapenShot();
               
            }
        }

        private void Jump()
        {
            if (inputFeedBack.isJumping)
            {
                playerMovement.Jump();
            }
        }

        private void Look()
        {
            playerRotation.ControlPlayerRotation(camera);
        }

        private void Interact()
        {
            if (inputFeedBack.isInteracting && VehicaleManager.Instance.canInteract)
            {
                VehicaleManager.Instance.InteractPlayerVehicleEnter();
                inputFeedBack.isInteracting = false;
            }
        }

        private void ChangeFireType()
        {
            if (inputFeedBack.isChangingFireType)
            {
                playerShoot.ChangeFireType();
                inputFeedBack.isChangingFireType = false;
            }
        }

        private void Move()
        {
            playerMovement.Move(inputFeedBack.isSprinting ,inputFeedBack.moveDirection);
        }

        private void StepsSound()
        {
            if (inputFeedBack.isSprinting)
            {
                playerFootSteps.volumeMin = sprint_volume;
                playerFootSteps.volumeMax = sprint_volume;
                playerFootSteps.stepsN = sprint_step_distance;
            }
            else
            {
                playerFootSteps.volumeMin = walk_volume_min;
                playerFootSteps.volumeMax = walk_volume_max;
                playerFootSteps.stepsN = walk_step_distance;
            }
        }

        

        

    }
}