using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunBullet_Laser : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private bool _isOn;
    [SerializeField] private bool _hasCausedDamage;
    [SerializeField] private float _damage = 100f;

    public Collider2D Collider => this._collider;
    public Animator Animator => this._animator;
    public LayerMask EnemyLayer => this._enemyLayer;

    public bool HasCausedDamage
    {
        get { return this._hasCausedDamage; }
        private set { this._hasCausedDamage = value; }
    }
    public bool IsOn
    {
        get { return this._isOn; }
        private set 
        {
            if (value)
            {
                this.Animator?.SetTrigger(AnimationString.isOn);
            }

            this._isOn = value; 
        }
    }
    public float Damage => this._damage;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadAnimation();
    }

    private void LoadAnimation()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
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
        this._hasCausedDamage = false;
        this._isOn = false;
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.IsOn = true;
    }

    public void BulletTrace()
    {
        Bounds bounds = this.Collider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.right, 0, this.EnemyLayer);
        if (hit.collider != null)
        {
            I_Damageable damageable = hit.collider.GetComponent<I_Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(this.Damage);
            }
        }
    }

    public void DestroyObject()
    {
        this.IsOn = false; 
        BulletSpawner.Instance.Destroy(this.transform);
    }

}
