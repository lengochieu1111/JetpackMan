using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveWidget : BaseWidget
{
    [SerializeField] private TextMeshProUGUI _timeCounter_Text;
    [SerializeField] private TextMeshProUGUI _allCoin_Text;
    [SerializeField] private int _crystalForRevival = 2;
    [SerializeField] private int _coinForRevival = 500;
    public TextMeshProUGUI TimeCounter_Text => this._timeCounter_Text;
    public TextMeshProUGUI AllCoin_Text => this._allCoin_Text;

    public int CoinForRevival => _coinForRevival;
    public int CrystalForRevival => _crystalForRevival;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadTimeCounter_Text();
        this.LoadAllCoin();
    }

    private void LoadTimeCounter_Text()
    {
        if (this._timeCounter_Text != null) return;

        Transform coinGold = this.FindChildByName(this.transform, "TimeCounter_Text");
        this._timeCounter_Text = coinGold?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadAllCoin()
    {
        if (this._allCoin_Text != null) return;

        Transform coin = this.FindChildByName(this.transform, "AllCoin_Text");
        this._allCoin_Text = coin?.GetComponent<TextMeshProUGUI>();
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

    protected override void OnEnable()
    {
        base.OnEnable();

        this.UpdateAllCoin_Text(GameMode.Instance.PlayerState.AllCoins.ToString());
    }

    /*
     * 
     */

    public void PressReviveByCoinButton()
    {
        if (GameMode.Instance.PlayerState.Reduce_Coin(this.CoinForRevival))
            this.Revive();
    }

    public void PressReviveByCrystalButton()
    {
        if (GameMode.Instance.PlayerState.Reduce_Crystal(this.CrystalForRevival))
            this.Revive();
    }

    private void Revive()
    {
        GameManager.Instance.SetMatchState(MatchState.InProgress);
    }

    /*
     * 
     */

    public void UpdateTimeCounter_Text(string text)
    {
        if (this.TimeCounter_Text == null) return;
        this.TimeCounter_Text.text = text;
    }

    public void UpdateAllCoin_Text(string text)
    {
        if (this.AllCoin_Text == null) return;
        this.AllCoin_Text.text = text;
    }

    /*
     * 
     */
}
