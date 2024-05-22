using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelIndexButton_LevelWidget : RyoMonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI _level_Text;
    [SerializeField] private Image _clockLevel_Image;
    public TextMeshProUGUI Level_Text => _level_Text;
    public Image ClockLevel_Image => _clockLevel_Image;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelText();
        this.LoadClockLevelImage();
    }

    private void LoadLevelText()
    {
        if (this._level_Text != null) return;
        this._level_Text = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void LoadClockLevelImage()
    {
        if (this._clockLevel_Image != null) return;
        this._clockLevel_Image = this.transform.Find("ClockLevel_Image")?.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlayAudio_Compress();
    }

    public void PressLevelIndexButton()
    {
        GetComponentInParent<LevelWidget>()?.LevelChange(this);
    }

    public void LevelOpen()
    {
        this.ClockLevel_Image.gameObject.SetActive(false);
        this.Level_Text.gameObject.SetActive(true);
    }

    public void LevelClosed()
    {
        this.ClockLevel_Image.gameObject.SetActive(true);
        this.Level_Text.gameObject.SetActive(false);
    }

}
