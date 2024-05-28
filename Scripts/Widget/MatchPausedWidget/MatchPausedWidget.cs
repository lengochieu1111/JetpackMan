using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MatchPausedWidget : BaseWidget
{
    [SerializeField] private Image _soundIcon_Image;
    [SerializeField] private Image _musicIcon_Image;
    [SerializeField] private Image _vibrationIcon_Image;
    [SerializeField] private Sprite _offSound_Sprite, _onSound_Sprite;
    [SerializeField] private Sprite _offMusic_Sprite, _onMusic_Sprite;
    [SerializeField] private Sprite _offVibration_Sprite, _onVibration_Sprite;
    public Image SoundIcon_Image => _soundIcon_Image;
    public Image MusicIcon_Image => _musicIcon_Image;
    public Image VibrationIcon_Image => _vibrationIcon_Image;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSoundIcon_Image();
        this.LoadMusicIcon_Image();
        this.LoadVibrationIcon_Image();
    }

    private void LoadSoundIcon_Image()
    {
        if (this._soundIcon_Image != null) return;
        this._soundIcon_Image = this.FindChildByName(this.transform, "SoundIcon_Image")?.GetComponent<Image>();
    }

    private void LoadMusicIcon_Image()
    {
        if (this._musicIcon_Image != null) return;
        this._musicIcon_Image = this.FindChildByName(this.transform, "MusicIcon_Image")?.GetComponent<Image>();
    }

    private void LoadVibrationIcon_Image()
    {
        if (this._vibrationIcon_Image != null) return;
        this._vibrationIcon_Image = this.FindChildByName(this.transform, "VibrationIcon_Image")?.GetComponent<Image>();
    }

    private Transform FindChildByName(Transform parrent, string childName)
    {
        Transform childObject = parrent.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrent)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }

    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        if (SoundManager.Instance.IsActive)
        {
            this.SetSprite_Sound(this._onSound_Sprite);
        }
        else
        {
            this.SetSprite_Sound(this._offSound_Sprite);
        }

        if (MusicManager.Instance.IsActive)
        {
            this.SetSprite_Music(this._onMusic_Sprite);
        }
        else
        {
            this.SetSprite_Music(this._offMusic_Sprite);
        }

    }

    /*
     * 
     */

    public void PressSoundButton()
    {
        if (SoundManager.Instance.IsActive)
        {
            SoundManager.Instance.PlayAudio_Compress();
        }

        SoundManager.Instance.ChangeActive();

        if (SoundManager.Instance.IsActive)
        {
            this.SetSprite_Sound(this._onSound_Sprite);
        }
        else
        {
            this.SetSprite_Sound(this._offSound_Sprite);
        }
    }

    public void PressMusicButton()
    {
        if (SoundManager.Instance.IsActive)
        {
            SoundManager.Instance.PlayAudio_Compress();
        }

        MusicManager.Instance.ChangeActive();

        if (MusicManager.Instance.IsActive)
        {
            this.SetSprite_Music(this._onMusic_Sprite);
        }
        else
        {
            this.SetSprite_Music(this._offMusic_Sprite);
        }
    }

    public void PressVibrationButton()
    {
        //if (SoundManager.Instance.IsActive)
        //{
        //    SoundManager.Instance.PlayAudio_Compress();
        //}

        Handheld.Vibrate();
    }

    public void PressResumeButton()
    {
        this.PlayAudioClicked();
        GameManager.Instance.SetMatchState(MatchState.InProgress);
    }

    public void PressRetryButton()
    {
        this.PlayAudioClicked();
        GameManager.Instance.SetMatchState(MatchState.InProgress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PressQuitButton()
    {
        GameManager.Instance.SetMatchState(MatchState.InProgress);
        GameManager.Instance.SetMatchState(MatchState.LeavingMatch);
    }

    /*
     * 
     */

    private void SetSprite_Sound(Sprite sprite)
    {
        this.SoundIcon_Image.sprite = sprite;
    }

    private void SetSprite_Music(Sprite sprite)
    {
        this.MusicIcon_Image.sprite = sprite;
    }

    private void SetSprite_Vibration(Sprite sprite)
    {
        this.VibrationIcon_Image.sprite = sprite;
    }
    /*
     * 
     */


    private void PlayAudioClicked()
    {
        SoundManager.Instance.PlayAudio_Compress();
    }

    //

}
