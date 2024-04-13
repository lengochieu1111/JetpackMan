using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : BaseCharacterAbstract
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private bool _isDead;
    [SerializeField] private BaseCharacterController _controller;
    public BaseCharacterController Controller => _controller;

    public float Health 
    { 
        get { return this._health; } 
        set { this._health = value; } 
    }
    public float MaxHealth 
    { 
        get { return this._maxHealth; } 
        set { this._maxHealth = value; } 
    }
    public bool IsDead
    {
        get { return this._isDead; }
        set
        {
            if (value)
            {
                this.Dead();
            }
            this._isDead = value;
        }
    }

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadController();
    }

    private void LoadController()
    {
        if (this._controller != null || this.character == null) return;

        this._controller = this.Character?.Controller;
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.Health = this.MaxHealth;
        this.IsDead = false;
    }


    public void TakeDamage(float damage)
    {
        this.Health = Mathf.Clamp(this.Health - damage, 0, this.MaxHealth);
        if (this.CheckDead())
        {
            this.IsDead = true;
        }
    }

    private bool CheckDead()
    {
        return this.Health <= 0;
    }

    private void Dead()
    {
        this.Controller?.RequestDead();
    }
    
    public void Revive()
    {
        this.Health = this.MaxHealth;
        this.IsDead = false;
    }

}
