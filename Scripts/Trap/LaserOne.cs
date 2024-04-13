using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserOne : RyoMonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private float _damage = 100f;
    public float Damage => _damage;
    public CapsuleCollider2D CapsuleCollider
    {
        get { return this._capsuleCollider; }
        private set { this._capsuleCollider = value; }
    }
    public SpriteRenderer[] Sprites
    {
        get { return this._sprites; }
        private set { this._sprites = value; }
    }

    #region Component & Value
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSprites();
        this.LoadCapsuleCollider();
    }

    private void LoadCapsuleCollider()
    {
        if (this.CapsuleCollider != null) return;
        this.CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void LoadSprites()
    {
        if (this._sprites.Count() > 0) return;
        this._sprites = this.GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this.CapsuleCollider.isTrigger = true;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(this.Damage);
        }
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            TrapSpawner.Instance.Destroy(this.transform);
        }

    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = true;
        bool canDestroy_2 = this.transform.position.x < Camera.main.transform.position.x;

        foreach(SpriteRenderer sprite in this._sprites)
        {
            if (sprite.isVisible)
            {
                canDestroy_1 = false;
                break;
            }
        }

        return canDestroy_1 & canDestroy_2;
    }

}
