using UnityEngine;

namespace Solders
{
    public class SolderSound : MonoBehaviour
    {
        [SerializeField] private AudioClip runClip;
        [SerializeField] private AudioClip shotClip;
        
        private AudioSource movementSource;
        private AudioSource actionSource;
        private bool isMoving;

        private float currentShotTime = 0;

        public float timeInterval = 1f;

        private void Awake()
        {
            movementSource = gameObject.AddComponent<AudioSource>();
            actionSource = gameObject.AddComponent<AudioSource>();
        }

        public void PlayRunSound(bool moving)
        {
            if(isMoving == moving) return;
            isMoving = moving;
            
            if(moving)
            {
                movementSource.clip = runClip;
                movementSource.loop = true;
                movementSource.Play();
            }
            else
            {
                movementSource.Stop();
            }
        }
        
        public void PlayShotSound()
        {
            if (Time.time > currentShotTime + timeInterval)
            {
                currentShotTime = Time.time;
                actionSource.PlayOneShot(shotClip);
            }
           // actionSource.PlayOneShot(shotClip);
        }
    }
}