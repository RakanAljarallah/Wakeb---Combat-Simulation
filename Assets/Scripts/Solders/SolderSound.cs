using System;
using UnityEngine;

namespace Solders
{
    public class SolderSound : MonoBehaviour
    {
        private AudioSource audioSource;
        
        [SerializeField] private AudioClip runClip;
        [SerializeField] private AudioClip shotClip;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRunSound()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(runClip);
            }
            
        }
        
        public void PlayShotSound()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(shotClip);
            }
        }
        
    }
}