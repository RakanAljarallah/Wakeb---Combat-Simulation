using System;
using System.Collections;
using UnityEngine;

public class VehicalSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip engineStartSound;
    [SerializeField] private AudioClip engineStopSound;
    [SerializeField] private AudioClip engineRunningSound;
    [SerializeField] private AudioClip engineAccelerationSound;
    
    public enum EngineSoundType
    {
        Start,
        Stop,
        Running,
        Acceleration
    }
    
    private EngineSoundType currentEngineSoundType;

    public EngineSoundType CurrentEngineSoundType
    {
        get => currentEngineSoundType;
        set
        {
             
            currentEngineSoundType = value;
            switch (currentEngineSoundType)
            {
                case EngineSoundType.Start:
                    StopAllCoroutines();
                   StartCoroutine(PlayEngineStartSound());
                    break;
                case EngineSoundType.Stop:
                    PlayEngineStopSound();
                    break;
                case EngineSoundType.Running:
                    StopAllCoroutines();
                    StartCoroutine(PlayEngineRunningSound());
                    break;
                case EngineSoundType.Acceleration:
                    StopAllCoroutines();
                    StartCoroutine( PlayEngineAccelerationSound());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public IEnumerator PlayEngineStartSound()
    {
        audioSource.clip = engineStartSound;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        CurrentEngineSoundType = EngineSoundType.Running;
    }
    
    public void PlayEngineStopSound()
    {
        //StartSound(engineStopSound);
        
    }
    
    public IEnumerator PlayEngineRunningSound()
    {
        while (currentEngineSoundType == EngineSoundType.Running)
        {
            
            
            audioSource.clip = engineRunningSound;
            audioSource.Play();
            
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        yield return null;
    }                    
    
    public IEnumerator PlayEngineAccelerationSound()
    {
        while (currentEngineSoundType == EngineSoundType.Acceleration)
        {
            
            audioSource.clip = engineAccelerationSound;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        yield return null;
    }
    
    // IEnumerator StartSound(AudioClip clip, bool overwrite = true)
    // {
    //     if (overwrite)
    //     {
    //         audioSource.clip = clip;
    //         audioSource.Play();
    //         yield return new WaitForSeconds(audioSource.clip.length);
    //     }
    //     else
    //     {
    //         yield return new WaitForSeconds(audioSource.clip.length);
    //         audioSource.clip = clip;
    //         audioSource.Play();
    //         
    //     }
    //     
    // }
}
