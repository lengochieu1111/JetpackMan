using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextButton_DeathWidget : RyoMonoBehaviour, IPointerDownHandler, IPointerUpHandler
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

    public void PressNextButton()
    {
        int sceneTotal = SceneManager.sceneCountInBuildSettings;
        int sceneNext = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(sceneNext % sceneTotal);
    }

}
