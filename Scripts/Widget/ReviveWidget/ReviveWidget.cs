using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveWidget : RyoMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeCounter_Text;
    public TextMeshProUGUI TimeCounter_Text
    {
        get { return this._timeCounter_Text; }
        private set { this._timeCounter_Text = value; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        Transform coinGold = this.transform.Find("TimeCounter_Text");
        this.TimeCounter_Text = coinGold?.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTimeCounter_Text(string text)
    {
        if (this.TimeCounter_Text == null) return;
        this.TimeCounter_Text.text = text;
    }
}
