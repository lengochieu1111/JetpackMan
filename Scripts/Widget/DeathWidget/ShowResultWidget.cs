using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowResultWidget : RyoMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _matchCoin_Text;
    [SerializeField] private TextMeshProUGUI _allCoind_Text;
    [SerializeField] private TextMeshProUGUI _matchDistance_Text;
    public TextMeshProUGUI MatchCoind_Text
    {
        get { return this._matchCoin_Text; }
        private set { this._matchCoin_Text = value; }
    }
    public TextMeshProUGUI AllCoind_Text
    {
        get { return this._allCoind_Text; }
        private set { this._allCoind_Text = value; }
    }
    public TextMeshProUGUI MatchDistance_Text
    {
        get { return this._matchDistance_Text; }
        private set { this._matchDistance_Text = value; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        Transform coind_Match = this.transform.Find("MatchCoin_Text");
        this.MatchCoind_Text = coind_Match?.GetComponent<TextMeshProUGUI>();

        Transform coind_All = this.transform.Find("AllCoin_Text");
        this.AllCoind_Text = coind_All?.GetComponent<TextMeshProUGUI>();

        Transform distance = this.transform.Find("MatchDistance_Text");
        this.MatchDistance_Text = distance?.GetComponent<TextMeshProUGUI>();

    }

    public void UpdateMatchCoind_Text(string text)
    {
        if (this.MatchCoind_Text == null) return;
        this.MatchCoind_Text.text = text;
    }

    public void UpdateAllCoind_Text(string text)
    {
        if (this.AllCoind_Text == null) return;
        this.AllCoind_Text.text = text;
    }

    public void UpdateMatchDistance_Text(string text)
    {
        if (this.MatchDistance_Text == null) return;
        this.MatchDistance_Text.text = text + " m";
    }
}
