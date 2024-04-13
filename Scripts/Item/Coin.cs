using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECoinType
{
    Gold,
    Silver
}

public class Coin : RyoMonoBehaviour
{
    [SerializeField] private ECoinType _coinType;
    [SerializeField] private float _timeToDestroy = 0.2f;
    private Collider2D _collider;
    private Coroutine _destroyCoroutine;
    public Collider2D Collider
    {
        get { return this._collider; }
        private set { this._collider = value; }
    }
    public float TimeToDestroy
    {
        get { return this._timeToDestroy; }
        private set { this._timeToDestroy = value; }
    }
    public ECoinType CoinType => _coinType;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
    }

    private void LoadCollider()
    {
        if (this.Collider != null) return;
        this.Collider = GetComponent<Collider2D>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.Collider != null)
        {
            this.Collider.isTrigger = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null)
        {
            receiveItem.PickUp_Coin(this.CoinType);

            this._destroyCoroutine = StartCoroutine(this.DestroyItem());
        }
    }
    
    private IEnumerator DestroyItem()
    {
        float countTime = 0;
        Vector3 localScale = this.transform.localScale;

        while (countTime < this.TimeToDestroy)
        {
            countTime += Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(localScale, Vector3.zero, countTime / this.TimeToDestroy);

            yield return null;
        }

        ItemSpawner.Instance.Destroy(this.transform);
        this.transform.localScale = localScale;
    }

}
