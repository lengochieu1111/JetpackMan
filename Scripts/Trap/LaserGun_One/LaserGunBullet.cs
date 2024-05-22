using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunBullet : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private bool _isFlying;
    [SerializeField] private float _speed = 16f;
    private float _currentSpeed;
    public Collider2D Collider => this._collider;
    public Animator Animator => this._animator;
    public SpriteRenderer Sprite => _sprite;
    public float CurrentSpeed
    {
        get { return this._currentSpeed; }
        private set { this._currentSpeed = value; }
    }
    public bool IsFlying
    {
        get { return this._isFlying; }
        private set { this._isFlying = value; }
    }
    public float Speed => this._speed;
    public float Damage => this._damage;

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

        this.CurrentSpeed = this.Speed;
        this.IsFlying = true;
    }
    #endregion

    protected override void OnDisable()
    {
        base.OnDisable();

        this.IsFlying = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            if (damageable.TakeDamage(this.Damage))
            {
                this.IsFlying = false;
                this.Animator.SetTrigger("isDead");

                StartCoroutine(this.DeadCoroutine());
            }
        }
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.Animator.SetTrigger("isRevive");
            TrapSpawner.Instance.Destroy(this.transform);
        }

        if (this.IsFlying)
        {
            this.Flying();
        }
    }

    private void Flying()
    {
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.transform.Translate(Vector2.right * this.CurrentSpeed * Time.deltaTime);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }

    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(1f);
        TrapSpawner.Instance.Destroy(this.transform);
    }

}
