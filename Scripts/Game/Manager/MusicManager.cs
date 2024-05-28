using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton_DontDestroyOnLoad<MusicManager>, IDataPersistence
{
    [Header("Audio Source")]
    [SerializeField] private bool _isActive;
    [SerializeField] private AudioSource _audioSource;
    public AudioSource AudioSource => this._audioSource;
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

        if (this._audioSource == null)
            this._audioSource = GetComponent<AudioSource>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isActive = false;
    }

    public void PlayAudio(AudioClip clip)
    {
        this.AudioSource?.PlayOneShot(clip);
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
        this.IsActive = data.IsActive_Music;
    }

    public void SaveGame(ref GameData data)
    {
        data.IsActive_Music = this.IsActive;
    }

}
