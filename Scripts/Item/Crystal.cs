using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : RyoMonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private float _timeToDestroy = 0.2f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _amplitude = 2f; // Độ lắc lư lên xuống
    [SerializeField] private float _frequency = 3f; // Tần số lắc lư
    private Coroutine _destroyCoroutine;
    private Vector3 _initScale;
    private float _timeCounter;
    public Collider2D Collider => this._collider; 
    public float TimeToDestroy => this._timeToDestroy;
    public float Speed => this._speed;
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
        this.LoadSprite();
    }

    private void LoadCollider()
    {
        if (this.Collider != null) return;
        this._collider = GetComponent<Collider2D>();
    }

    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
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
        this._timeCounter = 0;
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
            if (CanDestroy())
            {
                this.DestroyObject();
            }

            this.Flying();
        }
    }

    private void Flying()
    {
        this.transform.Translate(Vector2.right* this.Speed * Time.deltaTime);

        this._timeCounter += Time.deltaTime;
        float newY = Mathf.Sin((this._timeCounter - 0) * this._frequency) * this._amplitude;
        this.transform.position = new Vector2 (this.transform.position.x, newY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null && this.IsOn)
        {
            this.IsOn = false;
            receiveItem.PickUp_Crystal();

            this.SpawnEffect();
            this._destroyCoroutine = StartCoroutine(this.DestroyItem_Coroutine());
        }
    }

    private IEnumerator DestroyItem_Coroutine()
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

    private void DestroyObject()
    {
        ItemSpawner.Instance.Destroy(this.transform);
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

}
