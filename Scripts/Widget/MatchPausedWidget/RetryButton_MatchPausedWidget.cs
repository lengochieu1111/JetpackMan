using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RetryButton_MatchPausedWidget : RyoMonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    public AudioClip CompressClip => _compressClip;
    public AudioClip UncompressClip => _uncompressClip;
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

    public void PressRetryButton()
    {
        Debug.Log("Retry Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
