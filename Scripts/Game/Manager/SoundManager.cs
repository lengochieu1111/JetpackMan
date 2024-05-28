using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : Singleton_DontDestroyOnLoad<SoundManager>, IDataPersistence
{
    [Header("Audio Source")]
    [SerializeField] private bool _isActive;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    public AudioSource AudioSource => this._audioSource;
    public AudioClip CompressClip => _compressClip;
    public AudioClip UncompressClip => _uncompressClip;
    public bool IsActive
    {
        get { return _isActive; }
        private set 
        {
            if (this.AudioSource != null)
            {
                this.AudioSource.enabled = value;
            }

            _isActive = value; 
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._audioSource != null) return;

        this._audioSource = GetComponent<AudioSource>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // DataPersistenceManager.Instance.ReceiveData(this);
    }

    public void PlayAudio(AudioClip clip)
    {
        this.AudioSource?.PlayOneShot(clip);
    }

    public void PlayAudio_Compress()
    {
        this.AudioSource.volume = 0.4f;
        this.PlayAudio(this.CompressClip);
    }

    public void ChangeActive()
    {
        this.IsActive = !this.IsActive;
        DataPersistenceManager.Instance.ReceiveData(this);

    }

    /*
     * 
     */

    public void LoadGame(GameData data)
    {
        this.IsActive = data.IsActive_Sound;
    }

    public void SaveGame(ref GameData data)
    {
         data.IsActive_Sound = this.IsActive;
    }
}
