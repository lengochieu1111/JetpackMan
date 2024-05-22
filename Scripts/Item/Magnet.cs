using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : RyoMonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private bool _isSuckingCoin;
    [SerializeField] private float _suckingCoinTime = 010f;
    [SerializeField] private LayerMask _itemLayer;
    private Coroutine _destroyCoroutine;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _amplitude = 2f; // Độ lắc lư lên xuống
    [SerializeField] private float _frequency = 3f; // Tần số lắc lư
    private float _timeCounter;
    public float Speed => this._speed;
    public Collider2D Collider => this._collider;
    public float SuckingCoinTime => this._suckingCoinTime;
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }
    public bool IsSuckingCoin
    {
        get { return this._isSuckingCoin; }
        private set { this._isSuckingCoin = value; }
    }
    public SpriteRenderer Sprite => _sprite;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
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
        this._isSuckingCoin = false;
        this._timeCounter = 0;
        this._itemLayer = LayerMask.GetMask(LayerMaskString.ItemLayer);
    }

    private void Update()
    {
        if (this.IsOn)
        {
            if (CanDestroy())
            {
                this.DestroyObject();
            }

            this.Flying();
        }

        if (this.IsSuckingCoin)
        {
            this.CheckCollisionWithEnemy();
        }

    }

    private void Flying()
    {
        this.transform.Translate(Vector2.right * this.Speed * Time.deltaTime);

        this._timeCounter += Time.deltaTime;
        float newY = Mathf.Sin((this._timeCounter - 0) * this._frequency) * this._amplitude;
        this.transform.position = new Vector2(this.transform.position.x, newY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null)
        {
            this.IsOn = false;
            this.SpawnEffect();
            this.transform.SetParent(GameMode.Instance.Player.transform);
            transform.localPosition = new Vector2(0, 2);
            this._destroyCoroutine = StartCoroutine(this.DestroyItem());
        }
    }

    private IEnumerator DestroyItem()
    {
        this.IsSuckingCoin = true;
        yield return new WaitForSeconds(this.SuckingCoinTime);
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

    private void CheckCollisionWithEnemy()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(this.Collider.bounds.center, this.Collider.bounds.size * 10, 0, Vector2.right, 0, this._itemLayer);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i].collider.transform.GetComponent<Coin>()?.SetIsFollowing(true);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.Collider.bounds.center, this.Collider.bounds.size * 10);
    }

}
