using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayButtton : RyoMonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Property")]
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;
    public Image Image => _image;
    public Sprite Default => _default;
    public Sprite Pressed => _pressed;
    public AudioClip CompressClip => _compressClip;
    public AudioClip UncompressClip => _uncompressClip;
    public AudioSource Source => _source;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadImage();
        this.LoadAudioSource();
    }

    private void LoadImage()
    {
        if (this._image != null) return;

        this._image = GetComponent<Image>();
    }
    
    private void LoadAudioSource()
    {
        if (this._source != null) return;

        this._source = GetComponent<AudioSource>();
    }
    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        this.Image.sprite = Pressed;
        this.Source.PlayOneShot(this.CompressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.Image.sprite = Default;
        this.Source.PlayOneShot(this.UncompressClip);
    }

    public void IWasClicked()
    {
        this.PlayGame();
    }

    public void PlayGame()
    {
        // SceneManager.LoadScene("Scenes/Game");
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(nextScene % sceneCount, LoadSceneMode.Single);
    }

}
