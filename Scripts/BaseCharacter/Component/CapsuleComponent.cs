using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CapsuleComponent : BaseCharacterAbstract
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private BaseCharacterController _controller;
    [SerializeField] private ContactFilter2D _groundFilter;
    [SerializeField] private float _touchDistance = 0.12f;

    private RaycastHit2D[] _groundHit = new RaycastHit2D[5];
    private RaycastHit2D[] _ceilingHit = new RaycastHit2D[5];

    [SerializeField] private bool _isOnGround = true;
    [SerializeField] private bool _isOnCeiling;

    [SerializeField] private float _fGravityScale = 0f;
    [SerializeField] private float _fFallingGravityScale = 1f;
    [SerializeField] private float _fReduceGravity = 0.01f;
    [SerializeField] private float _fCurrentGravityScale;
    public BaseCharacterController Controller => this._controller;
    public bool IsOnGround
    {
        get { return _isOnGround; }
        private set { _isOnGround = value; }
    }
    public bool IsOnCeiling
    {
        get { return _isOnCeiling; }
        private set { _isOnCeiling = value; }
    }
    public float TouchDistance => this._touchDistance;
    
    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadController();
        this.LoadCapsuleCollider();
        this.LoadRigidbody();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = this.Character?.CapsuleCollider;
    }

    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;

        this._rigidbody = this.Character?.Rigidbody;
    }
    
    private void LoadController()
    {
        if (this._controller != null) return;

        this._controller = this.Character?.Controller;
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsOnGround = true;
        this.IsOnCeiling = false;
        this._groundFilter.useLayerMask = true;
        this._groundFilter.layerMask = LayerMask.GetMask("GroundLayer");
    }

    private void FixedUpdate()
    {
        if (!this.IsOnGround && !this.Controller.IsDead)
        {
            this.GravityDecreasing();
        }
    }

    private void Update()
    {
        this.CheckIsOnGround();
        this.CheckIsCeiling();

        this.ChangeGravity();
    }

    private bool CheckIsOnGround()
    {
        return this.IsOnGround = this._capsuleCollider.Cast(Vector2.down, this._groundFilter, this._groundHit, this.TouchDistance) > 0;
    }

    private bool CheckIsCeiling()
    {
        return this.IsOnCeiling = this._capsuleCollider.Cast(Vector2.up, this._groundFilter, this._ceilingHit, this.TouchDistance) > 0;
    }
    
    private void ChangeGravity()
    {
        if ((this.IsOnGround && !this.IsOnCeiling) || this.Controller.IsRushingUpHigh)
            this._fCurrentGravityScale = this._fGravityScale;
        else
            this._fCurrentGravityScale = this._fFallingGravityScale;
    }

    private void GravityDecreasing()
    {
        this._rigidbody.AddForce(Physics.gravity * (this._fCurrentGravityScale - this._fReduceGravity) * this._rigidbody.mass);
    }

    public void RequestLanding()
    {
        this.IsOnGround = true;
        this.IsOnCeiling = false;
        this._fCurrentGravityScale = this._fGravityScale;
        this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, 0);
    }


    /*    private void OnDrawGizmos()
        {
            Bounds CapsuleBounds = this._capsuleCollider.bounds;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(CapsuleBounds.center + Vector3.down * 1.5f, CapsuleBounds.size);
        }*/
}
