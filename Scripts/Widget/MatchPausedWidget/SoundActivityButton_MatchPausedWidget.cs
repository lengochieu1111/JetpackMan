using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundActivityButton_MatchPausedWidget : RyoMonoBehaviour, IPointerDownHandler, IPointerUpHandler
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

    public void PressSoundActivityButton()
    {
        this.IsActive = !this.IsActive;
    }

}
