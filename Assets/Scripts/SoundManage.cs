using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpAudio, hurtAudio, getAudio, attactAudio, shootAudio;
    public static SoundManage instance;

    private void Awake()
    {
        instance = this;
    }

    public void JumpAudioPlay()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void GetAudioPlay()
    {
        audioSource.clip = getAudio;
        audioSource.Play();
    }

    public void HurtAudioPlay()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }    
    
    public void AttactAudioPlay()
    {
        audioSource.clip = attactAudio;
        audioSource.Play();
    }    
    
    public void ShootAudioPlay()
    {
        audioSource.clip = shootAudio;
        audioSource.Play();
    }
}
