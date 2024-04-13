using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton_DontDestroyOnLoad<MusicManager>
{
    [Header("Audio Source")]
    private AudioSource _audioSource;
    public AudioSource AudioSource => this._audioSource;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._audioSource == null)
            this._audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        this.AudioSource?.PlayOneShot(clip);
    }
}
