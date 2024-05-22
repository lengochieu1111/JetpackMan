using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCoin : RyoMonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float _timeToDestroy = 0.2f;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private LayerMask _playerLayer;
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
        if (this.Rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsOn = true;
        this._initScale = this.transform.localScale;
        this._playerLayer = LayerMask.GetMask(LayerMaskString.PlayerLayer);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        float randomX = Random.Range(500f, 800f);
        float randomY = Random.Range(-50f, -150f);
        this.Rigidbody?.AddForce(new Vector2(randomX, randomY));
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        this.transform.localScale = this._initScale;
    }

    private void Update()
    {
        if (this.IsOn)
        {
            if (this.CanDestroy())
            {
                this.DestroyObject();
            }

            this.CollisionWithPlayer();
        }
    }

    private void CollisionWithPlayer()
    {
        Bounds bounds = this.Collider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.left, 0, this._playerLayer);
        if (hit.collider != null)
        {
            I_PickUpItem receiveItem = hit.collider.transform.GetComponent<I_PickUpItem>();
            if (receiveItem != null)
            {
                this.IsOn = false;
                receiveItem.PickUp_Coin();

                this.SpawnEffect();
                this._destroyCoroutine = StartCoroutine(this.DestroyItem());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerOfCollidedObject = collision.gameObject.layer;
        if (LayerMask.LayerToName(layerOfCollidedObject) == LayerMaskString.GroundLayer)
        {
            float randomX = Random.Range(0f, 3f);
            float randomY = Random.Range(2f, 10f);
            this.Rigidbody?.AddForce(new Vector2(randomX, randomY), ForceMode2D.Impulse);
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
