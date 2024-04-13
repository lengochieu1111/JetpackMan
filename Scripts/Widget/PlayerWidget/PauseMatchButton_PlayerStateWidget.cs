using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMatchButton_PlayerStateWidget : RyoMonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    public AudioClip CompressClip => _compressClip;
    public AudioClip UncompressClip => _uncompressClip;

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.AudioSource.volume = 0.02f;
        SoundManager.Instance.PlayAudio(this.CompressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance.AudioSource.volume = 0.02f;
        SoundManager.Instance.PlayAudio(this.UncompressClip);
    }

    public void PressPauseButton()
    {
        GameManager.Instance.SetMatchState(MatchState.IsPaused);
    }

}
