using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunBullet_Broad : RyoMonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private bool _isOn;
    [SerializeField] private bool _hasCausedDamage;
    [SerializeField] private float _speed = 16f;
    private float _currentSpeed;
    private Coroutine _scaleCoroutine;

    public Collider2D Collider => this._collider;
    public SpriteRenderer Sprite => _sprite;
    public LayerMask EnemyLayer => this._enemyLayer;
    public float CurrentSpeed
    {
        get { return this._currentSpeed; }
        private set { this._currentSpeed = value; }
    }
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }
    public bool HasCausedDamage
    {
        get { return this._hasCausedDamage; }
        private set { this._hasCausedDamage = value; }
    }
    public float Speed => this._speed;
    public float Damage => this._damage;

    #region Load Component
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
        if (this._collider != null) return;
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

        this._enemyLayer = LayerMask.GetMask(LayerMaskString.EnemyLayer);
        this._isOn = false;
        this._hasCausedDamage = false;
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.CurrentSpeed = this.Speed;
        this.IsOn = true;
        this._scaleCoroutine = StartCoroutine(this.ScaleCoroutine());

    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.DestroyObject();
        }

        if (this.IsOn)
        {
            this.Flying();
        }
    }

    private void Flying()
    {
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.transform.Translate(Vector2.right * this.CurrentSpeed * Time.deltaTime);
    }

    private IEnumerator ScaleCoroutine()
    {
        float counter = 0;

        while (counter <= 1f)
        {
            counter += Time.deltaTime;
            float newScale = Mathf.Lerp(0.4f, 1, counter / 1f);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }
    }

    //public void BulletTrace()
    //{
    //    if (this.HasCausedDamage) return;

    //    Bounds bounds = this.Collider.bounds;
    //    RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.right, 0, this.EnemyLayer);
    //    if (hit.collider != null)
    //    {
    //        I_Damageable damageable = hit.collider.GetComponent<I_Damageable>();
    //        if (damageable != null )
    //        {
    //            bool successfullyCausedDamage = damageable.TakeDamage(this.Damage);
    //            if (successfullyCausedDamage)
    //            {
    //                this.DestroyObject();
    //            }

    //        }
    //    }
    //}

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x > CameraManager.Instance.RightCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }

    private void DestroyObject()
    {
        this.IsOn = false;
        this.HasCausedDamage = true;
        BulletSpawner.Instance.Destroy(this.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canCauseDamage = collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskString.EnemyLayer);
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null && canCauseDamage)
        {
            if (damageable.TakeDamage(this.Damage))
            {
                this.DestroyObject();
            }
        }
    }

}
