using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBullet : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private LayerMask _trapLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private bool _isOn = true;
    [SerializeField] private float _damage = 100f;

    [SerializeField] private float[] _speed = { 20f, 30f };
    private float _currentSpeed;
    public Collider2D Collider => this._collider;
    public Animator Animator => this._animator;
    public SpriteRenderer Sprite => _sprite;
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
    public float Damage => this._damage;
    public float[] Speed => this._speed;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadAnimation();
        this.LoadSprite();
    }

    private void LoadAnimation()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
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

        this._isOn = true;
        this._trapLayer = LayerMask.GetMask(LayerMaskString.TrapLayer);
        this._enemyLayer = LayerMask.GetMask(LayerMaskString.EnemyLayer);
        this._currentSpeed = Random.Range(this._speed[0], this._speed[1]);
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.Animator.SetBool(AnimationString.isOn, true);
    }

    private void Update()
    {
        if (this.CanDestroy() && this.IsOn)
        {
            this.DestroyObject();
        }

        if (this.IsOn)
        {
            this.Flying();
        }

        if (this.IsOn)
        {
            this.CheckCollisionWithTrap();
            this.CheckCollisionWithEnemy();
        }

    }

    private void Flying()
    {
        // Cập nhật tốc độ
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.transform.Translate(Vector2.right * this.CurrentSpeed * Time.deltaTime);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x > CameraManager.Instance.RightCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }


    public void DestroyObject()
    {
        this.Effect();
        BulletSpawner.Instance.Destroy(this.transform);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canCauseDamage = collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskString.EnemyLayer);
        if (canCauseDamage && this.IsOn)
        {
            I_Damageable damageable = collision.GetComponent<I_Damageable>();
            if (damageable != null)
            {
                if (damageable.TakeDamage(this.Damage))
                {
                    this.IsOn = false;
                    this.DestroyObject();
                }
            }
        }
    }

    private void CheckCollisionWithTrap()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(this.Collider.bounds.center, this.Collider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.right, 0, this._trapLayer);
        if (hit.collider != null && this.IsOn)
        {
            TrapSpawner.Instance.Destroy(hit.collider.transform);
            this.IsOn = false;
            this.DestroyObject();
        }

    }
    
    private void CheckCollisionWithEnemy()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(this.Collider.bounds.center, this.Collider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.right, 0, this._enemyLayer);
        if (hit.collider != null && this.IsOn)
        {
            I_Damageable damageable = hit.collider.transform.GetComponent<I_Damageable>();
            if (damageable != null)
            {
                if (damageable.TakeDamage(this.Damage))
                {
                    this.IsOn = false;
                    this.DestroyObject();
                }
            }


        }

    }

    private void Effect()
    {
        Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.ExplosionOne, this.transform.position, this.transform.rotation);
        effect.gameObject.SetActive(true);
    }


}
