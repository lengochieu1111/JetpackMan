using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCoin : RyoMonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private float _timeToDestroy = 0.4f;
    private Coroutine _destroyCoroutine;
    private Vector3 _initScale;
    public Collider2D Collider => this._collider;
    public Rigidbody2D Rigidbody => this._rigidbody;
    public float TimeToDestroy => this._timeToDestroy;
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }
    public SpriteRenderer Sprite => _sprite;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadRigidbody();
        this.LoadSprite();
    }

    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
    }

    private void LoadCollider()
    {
        if (this.Collider != null) return;
        this._collider = GetComponent<Collider2D>();
    }
    
    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
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
        this._initScale = this.transform.localScale;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.Rigidbody.gravityScale = 0;
        this.Rigidbody.velocity = Vector3.zero;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.transform.localScale = this._initScale;
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.DestroyObject();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null)
        {
            this.IsOn = false;
            receiveItem.PickUp_Coin();

            this.SpawnEffect();

            this._destroyCoroutine = StartCoroutine(this.DestroyItem());
        }
    }

    private IEnumerator DestroyItem()
    {
        this.Rigidbody.gravityScale = 1f;
        this.Rigidbody?.AddForce(new Vector2(1500f, -200f));

        yield return new WaitForSeconds(0.1f);

        float countTime = 0;
        while (countTime < this.TimeToDestroy)
        {
            countTime += Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(this._initScale, Vector3.zero, countTime / this.TimeToDestroy);

            Vector3 spawnPos = this.transform.position;
            spawnPos.x += 1;
            Transform poolObject = ItemSpawner.Instance.Spawn(ItemSpawner.DynamicCoin, spawnPos, transform.rotation);
            poolObject.gameObject.SetActive(true);

            yield return null;
        }

        this.DestroyObject();
    }

    private void SpawnEffect()
    {
        Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.GoldCoinPickUp, this.transform.position, this.transform.rotation);
        effect.gameObject.SetActive(true);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }

    private void DestroyObject()
    {
        ItemSpawner.Instance.Destroy(this.transform);
    }

}
