using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public enum CoinMeshType
{
    Heart
}

public class CoinMesh : RyoMonoBehaviour
{
    [SerializeField] private CoinMeshType _coinMeshType;
    [SerializeField] private List<List<Coin>> CoinList = new List<List<Coin>>();
    [SerializeField] private List<List<Coin>> CoinList_AfterCollision = new List<List<Coin>>();
    [SerializeField] private float _delayTime = 0.01f;
    [SerializeField] private bool _isEnable = true;
    private int _maximumColumns;
    private List<Coin> _firstRowCoin = new List<Coin>();
    private Coroutine _showCoinCoroutine;
    public bool IsEnable
    {
        get { return this._isEnable; }
        private set { this._isEnable = value; }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCoinMesh();
    }

    private void LoadCoinMesh()
    {
        int childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform chill = this.transform.GetChild(i);
            List<Coin> row = chill.GetComponentsInChildren<Coin>().ToList();
            this.CoinList.Add(row);
        }

        this._firstRowCoin = this.transform.GetChild(0).GetComponentsInChildren<Coin>(true).ToList();
        this._maximumColumns = this._firstRowCoin.Count;

        foreach (List<Coin> coinRow in this.CoinList)
        {
            foreach (Coin coin in coinRow)
            {
                coin.gameObject.SetActive(false);
            }
        }

    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsEnable = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this._showCoinCoroutine = StartCoroutine(this.ShowCoinCoroutine());
    }

    private void Update()
    {
        if (this.CanDestroy() && this.IsEnable)
        {
            this.IsEnable = false;
            this.DestroyCoinMesh();
        }
    }

    private IEnumerator ShowCoinCoroutine()
    {
        for (int i = 0; i < this._maximumColumns; i++)
        {
            float xAxis = this._firstRowCoin[i].transform.localPosition.x;

            foreach (List<Coin> coinRow in this.CoinList)
            {
                foreach (Coin coin in coinRow)
                {
                    if (coin.transform.localPosition.x == xAxis)
                        coin.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(this._delayTime);
        }
    }

    private bool CanDestroy()
    {
        float xAxis = this._firstRowCoin[this._maximumColumns - 1].transform.position.x;
        if (xAxis + 5 < CameraManager.Instance.LeftCornerOfCamera.transform.position.x)
            return true;

        else return false;
    }

    private void DestroyCoinMesh()
    {
        foreach (List<Coin> coinRow in this.CoinList)
        {
            foreach (Coin coin in coinRow)
            {
                coin.Revive();
            }
        }

        ItemSpawner.Instance.Destroy(this.transform);
    }

    //private void LoadCoinMesh_AfterCollision()
    //{
    //    int childCount = this.transform.childCount;

    //    for (int i = 0; i < childCount; i++)
    //    {
    //        Transform chill = this.transform.GetChild(i);
    //        List<Coin> row = chill.GetComponentsInChildren<Coin>().ToList();
    //        this.CoinList_AfterCollision.Add(row);
    //    }

    //    this._firstRowCoin = this.transform.GetChild(0).GetComponentsInChildren<Coin>(true).ToList();
    //    this._maximumColumns = this._firstRowCoin.Count;

    //    foreach (List<Coin> coinRow in this.CoinList)
    //    {
    //        foreach (Coin coin in coinRow)
    //        {
    //            coin.gameObject.SetActive(false);
    //        }
    //    }

    //}


}
