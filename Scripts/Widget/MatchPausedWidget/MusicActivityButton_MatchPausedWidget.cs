using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicActivityButton_MatchPausedWidget : RyoMonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    public Image Image => _image;
    public AudioClip CompressClip => _compressClip;
    public AudioClip UncompressClip => _uncompressClip;
    public bool IsActive
    {
        get { return this._isActive; }
        private set 
        {
            if (value)
                this.Image.sprite = this._onSprite;
            else
                this.Image.sprite = this._offSprite;

            this._isActive = value; 
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadImage();
    }

    private void LoadImage()
    {
        if (this._image != null) return;
        this._image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.AudioSource.volume = 0.06f;
        SoundManager.Instance.PlayAudio(this.CompressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.AudioSource.volume = 0.06f;
        SoundManager.Instance.PlayAudio(this.UncompressClip);
    }

    public void PressMusicActivityButton()
    {
        this.IsActive = !this.IsActive;

        // Handle music activity status
    }
}
