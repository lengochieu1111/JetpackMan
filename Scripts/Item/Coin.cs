using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : RyoMonoBehaviour
{
    [SerializeField] private bool _isFollowing = false;
    [SerializeField] private float _followSpeed = 0.03f;
    [SerializeField] private float _timeToDestroy = 0.2f;
    [SerializeField] private bool _isOn = true;
    private Collider2D _collider;
    private Coroutine _destroyCoroutine;
    private Vector3 _initScale;
    public Collider2D Collider => this._collider; 
    public float TimeToDestroy => this._timeToDestroy; 
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
    }

    private void LoadCollider()
    {
        if (this.Collider != null) return;
        this._collider = GetComponent<Collider2D>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.Collider != null)
        {
            this.Collider.isTrigger = true;
        }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsOn = true;
        this._isFollowing = false;
        this._initScale = this.transform.localScale;
    }

    private void Update()
    {
        if (this._isFollowing)
        {
            this._followSpeed += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, GameMode.Instance.Player.transform.position, this._followSpeed);
        }
    }

    public void Revive()
    {
        this.transform.localScale = this._initScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null && this.IsOn)
        {
            this.IsOn = false;
            receiveItem.PickUp_Coin();

            this.SpawnEffect();
            this._destroyCoroutine = StartCoroutine(this.DestroyItem());
        }
    }
    
    private IEnumerator DestroyItem()
    {
        float countTime = 0;

        while (countTime < this.TimeToDestroy)
        {
            countTime += Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(this._initScale, Vector3.zero, countTime / this.TimeToDestroy);

            yield return null;
        }

        // ItemSpawner.Instance.Destroy(this.transform);
        // this.gameObject.SetActive(false);
    }

    private void SpawnEffect()
    {
        Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.GoldCoinPickUp, this.transform.position, this.transform.rotation);
        effect.gameObject.SetActive(true);
    }

    public void SetIsFollowing(bool isFollowing)
    {
        this._isFollowing = isFollowing;
    }

}
