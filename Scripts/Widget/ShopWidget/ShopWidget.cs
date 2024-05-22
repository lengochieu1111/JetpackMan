using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemStatus
{
    Opened, 
    Unopened, 
    InUse
}

public class ShopWidget : RyoMonoBehaviour, IDataPersistence
{
    [SerializeField] private Item_Shop _itemOne, _itemTwo, _itemThree;
    [SerializeField] private TextMeshProUGUI _coin_Text;
    [SerializeField] private TextMeshProUGUI _crystal_Text;
    [SerializeField] private int _coin;
    [SerializeField] private int _crystal;

    [SerializeField] private List<bool> _openedItems = new List<bool> { true, false, false };
    [SerializeField] private int _currentBullet;
    [SerializeField] private int _currentCharacter;

    [SerializeField] private int _priceOfItem_One = 2000;
    [SerializeField] private int _priceOfItem_Two = 10;
    [SerializeField] private int _priceOfItem_Three = 50;
    public int PriceOfItem_One => _priceOfItem_One;
    public int PriceOfItem_Two => _priceOfItem_Two;
    public int PriceOfItem_Three => _priceOfItem_Three;
    public List<bool> OpenedItems
    {
        get { return _openedItems; }
        private set 
        { 
            // this.SetupOpenedItems();
            _openedItems = value; 
        }
    }
    public int CurrentBullet
        {
        get { return this._currentBullet; }
        private set { this._currentBullet = value; }
    }
    public int CurrentCharacter
    {
        get { return this._currentCharacter; }
        private set { this._currentCharacter = value; }
    }
    public int Coin
    {
        get { return this._coin; }
        private set
        {
            if (this.Coin_Text != null)
            {
                this.Coin_Text.text = value.ToString();
            }

            this._coin = value;
        }
    }
    public int Crystal
    {
        get { return this._crystal; }
        private set
        {
            if (this.Crystal_Text != null)
            {
                this.Crystal_Text.text = value.ToString();
            }

            this._crystal = value;
        }
    }
    public TextMeshProUGUI Coin_Text => _coin_Text;
    public TextMeshProUGUI Crystal_Text => _crystal_Text;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCoin_Text();
        this.LoadCrystal_Text();

        this.LoadItemOne();
        this.LoadItemTwo();
        this.LoadItemThree();
    }

    private void LoadCoin_Text()
    {
        if (this._coin_Text != null) return;
        this._coin_Text = this.FindChildByName(this.transform, "Coin_Text")?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadCrystal_Text()
    {
        if (this._crystal_Text != null) return;
        this._crystal_Text = this.FindChildByName(this.transform, "Crystal_Text")?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadItemOne()
    {
        if (this._itemOne != null) return;
        this._itemOne = this.FindChildByName(this.transform, "Item_1")?.GetComponent<Item_Shop>();
    }
    private void LoadItemTwo()
    {
        if (this._itemTwo != null) return;
        this._itemTwo = this.FindChildByName(this.transform, "Item_2")?.GetComponent<Item_Shop>();
    }
    private void LoadItemThree()
    {
        if (this._itemThree != null) return;
        this._itemThree = this.FindChildByName(this.transform, "Item_3")?.GetComponent<Item_Shop>();
    }

    private Transform FindChildByName(Transform parrentObject, string childName)
    {
        Transform childObject = parrentObject.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrentObject)
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

        DataPersistenceManager.Instance.SendData(this);

        //
        this.SetupOpenedItems();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        DataPersistenceManager.Instance.ReceiveData(this);
    }

    /*
     * 
     */

    public void PressExitButton()
    {
        this.gameObject.SetActive(false);
    }


    // ItemOne

    public void PressItemOneButton_Unopened()
    {
        if (this.Coin >= this.PriceOfItem_One)
        {
            this.Coin -= this.PriceOfItem_One;
            this.OpenedItems[0] = true;
            this._itemOne?.Opened();
            DataPersistenceManager.Instance.ReceiveData(this);
        }
    }

    public void PressItemOneButton_Opened()
    {
        this.CurrentBullet = 1;
        this._itemOne?.InUse();
        
        if (this.OpenedItems[1])
        {
            this._itemTwo?.Opened();
        }
    }
    
    public void PressItemOneButton_InUse()
    {
        this.CurrentBullet = 0;
        this._itemOne?.Opened();
    }

    //
     
    public void PressItemTwoButton_Unopened()
    {
        if (this.Crystal >= this.PriceOfItem_Two)
        {
            this.Crystal -= this.PriceOfItem_Two;
            this.OpenedItems[1] = true;
            this._itemTwo?.Opened();
            DataPersistenceManager.Instance.ReceiveData(this);
        }
    }

    public void PressItemTwoButton_Opened()
    {
        this.CurrentBullet = 2;
        this._itemTwo?.InUse();

        if (this.OpenedItems[0])
        {
            this._itemOne?.Opened();
        }
    }

    public void PressItemTwoButton_InUse()
    {
        this.CurrentBullet = 0;
        this._itemTwo?.Opened();
    }

    //

    public void PressItemThreeButton_Unopened()
    {
        if (this.Crystal >= this.PriceOfItem_Three)
        {
            this.Crystal -= this.PriceOfItem_Three;
            this.OpenedItems[2] = true;
            this._itemThree?.Opened();
            DataPersistenceManager.Instance.ReceiveData(this);
        }
    }

    public void PressItemThreeButton_Opened()
    {
        this.CurrentCharacter = 1;
        this._itemThree?.InUse();
    }

    public void PressItemThreeButton_InUse()
    {
        this.CurrentCharacter = 0;
        this._itemThree?.Opened();
    }

    /*
     * 
     */

    #region Data Persistence
    public void LoadGame(GameData data)
    {
        this.Coin = data.AllCoins;
        this.Crystal = data.AllCrystals;
        this.CurrentBullet = data.CurrentBullet;
        this.CurrentCharacter = data.CurrentCharacter;
        this.OpenedItems = data.OpenedItems;
    }

    public void SaveGame(ref GameData data)
    {
        data.AllCoins = this.Coin;
        data.AllCrystals = this.Crystal;
        data.CurrentBullet = this.CurrentBullet;
        data.CurrentCharacter = this.CurrentCharacter;
        data.OpenedItems = this.OpenedItems;
    }
    #endregion

    /*
     * 
     */

    private void SetupOpenedItems()
    {
        if (this.OpenedItems[0])
        {
            this._itemOne?.Opened();

            if (this.CurrentBullet == 1)
            {
                this._itemOne?.InUse();
            }
        }
        else
        {
            this._itemOne?.Unopened();
        }

        if (this.OpenedItems[1])
        {
            this._itemTwo?.Opened();

            if (this.CurrentBullet == 2)
            {
                this._itemTwo?.InUse();
            }
        }
        else
        {
            this._itemTwo?.Unopened();
        }

        if (this.OpenedItems[2])
        {
            this._itemThree?.Opened();

            if (this.CurrentCharacter == 1)
            {
                this._itemThree?.InUse();
            }
        }
        else
        {
            this._itemThree?.Unopened();
        }

    }

}
