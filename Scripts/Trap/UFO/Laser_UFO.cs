using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Laser_UFO : RyoMonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _damage = 100f;
    public CapsuleCollider2D CapsuleCollider => _capsuleCollider;
    public Animator Animator => _animator;
    public float Damage => _damage;

    #region Component & Value
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadAnimators();
        this.LoadCapsuleCollider();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void LoadAnimators()
    {
        if (this._animator != null) return;
        this._animator = this.GetComponent<Animator>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this.CapsuleCollider.isTrigger = true;
        this.CapsuleCollider.enabled = false;
    }
    #endregion

    public void SetActiveStatus_Animator(bool active)
    {
        this.Animator.SetBool(AnimationString.isOn, active);
    }

    public void SetActiveStatus_CapsuleCollider(bool active)
    {
        this.CapsuleCollider.enabled = active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool canCauseDamage = collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskString.PlayerLayer);
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null && canCauseDamage)
        {
            damageable.TakeDamage(this.Damage);
        }
    }
}
