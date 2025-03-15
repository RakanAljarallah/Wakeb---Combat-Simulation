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
            audioSource.PlayOneShot(runClip);
        }
        
        public void PlayShotSound()
        {
            audioSource.PlayOneShot(shotClip);
        }
        
    }
}