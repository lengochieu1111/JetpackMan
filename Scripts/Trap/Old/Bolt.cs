using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : RyoMonoBehaviour
{
    [SerializeField] private PolygonCollider2D _polygonCollider;
    [SerializeField] private float _damage = 100f;
    public float Damage => _damage;
    public PolygonCollider2D PolygonCollider
    {
        get { return this._polygonCollider; }
        set { this._polygonCollider = value; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadPolygonCollider();
    }

    private void LoadPolygonCollider()
    {
        if (this.PolygonCollider != null) return;
        this.PolygonCollider = GetComponent<PolygonCollider2D>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.PolygonCollider != null)
        {
            this.PolygonCollider.isTrigger = true;
            this.PolygonCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(this.Damage);
        }
    }

}
