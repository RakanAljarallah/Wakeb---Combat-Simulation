
using UnityEngine;

namespace Player
{
    public class PlayerFootSteps : MonoBehaviour
    {
        private AudioSource audioSource;
        private CharacterController characterController;
        [SerializeField] 
        private AudioClip[] footsteps;
        
        [HideInInspector]
        public float volumeMin, volumeMax;
        
        private float mvoedDistance;

        public float stepsN;
        
        
        
        

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
          PlayFootStepSound();
        }
        
        public void PlayFootStepSound()
        {
            if (!characterController.isGrounded) return;
            if (characterController.velocity.sqrMagnitude > 0)
            {
                mvoedDistance += Time.deltaTime;
                if (mvoedDistance > stepsN)
                {
                    audioSource.volume = Random.Range(volumeMin, volumeMax);
                    audioSource.clip = footsteps[Random.Range(0, footsteps.Length)];
                    audioSource.Play();
                    mvoedDistance = 0;
                }
            } 
            else
            {
                mvoedDistance = 0;
            }
        }
    }
}