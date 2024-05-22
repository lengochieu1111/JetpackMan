using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserGun_One_Type
{
    Down,
    Top
}

public class LaserGun_One : RyoMonoBehaviour
{
    [SerializeField] private LaserGun_One_Type _laserGun_One_Type;
    [SerializeField] private Animator _animator;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private bool _isSeePlayer;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _topSpawn = 4.338215f;
    [SerializeField] private float _downSpawn = -5.898214f;

    [SerializeField] private float _colliderHeight_Down = 10f;
    [SerializeField] private float _colliderWidth_Down = 25f;
    [SerializeField] private float _colliderHeight_Top = 10f;
    [SerializeField] private float _colliderWidth_Top = 25f;
    private int _patrolDirection = 1;
    private float _colliderHeight;
    private float _colliderWidth;

    private float _currentSpeed;
    public LaserGun_One_Type LaserGun_One_Type => _laserGun_One_Type;
    public CapsuleCollider2D CapsuleCollider => this._capsuleCollider;
    public Animator Animator => this._animator;
    public SpriteRenderer Sprite => _sprite;
    public float CurrentSpeed
    {
        get { return this._currentSpeed; }
        private set { this._currentSpeed = value; }
    }
    public bool IsAttacking
    {
        get { return this._isAttacking; }
        private set 
        { 
            if (value)
            {
                this.Attack();
            }
            this._isAttacking = value; 
        }
    }
    public bool IsSeePlayer
    {
        get { return this._isSeePlayer; }
        private set { this._isSeePlayer = value; }
    }

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadColliderCapsuleCollider();
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

    private void LoadColliderCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    #endregion

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.CapsuleCollider != null)
        {
            this.CapsuleCollider.isTrigger = true;
        }

        if (this.LaserGun_One_Type == LaserGun_One_Type.Down)
        {
            this.transform.position = new Vector2(this.transform.position.x, this._downSpawn);
        }
        else if (this.LaserGun_One_Type == LaserGun_One_Type.Top)
        {
            this.transform.position = new Vector2(this.transform.position.x, this._topSpawn);
        }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsAttacking = false;
        this.IsSeePlayer = false;
        this._playerLayerMask = LayerMask.GetMask("PlayerLayer");

        if (this.LaserGun_One_Type == LaserGun_One_Type.Down)
        {
            this._patrolDirection = 1;
            this._colliderHeight = this._colliderHeight_Down;
            this._colliderWidth = this._colliderWidth_Down;
        }
        else if (this.LaserGun_One_Type == LaserGun_One_Type.Top)
        {
            this._patrolDirection = -1;
            this._colliderHeight = this._colliderHeight_Top;
            this._colliderWidth = this._colliderWidth_Top;
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    I_Damageable damageable = collision.GetComponent<I_Damageable>();
    //    if (damageable != null)
    //    {
    //        if (damageable.TakeDamage(this.Damage))
    //        {
    //            this.IsAttacking = false;
    //            this.Animator.SetTrigger("isDead");

    //            StartCoroutine(this.DeadCoroutine());
    //        }
    //    }
    //}

    private void Update()
    {
        if (this.CanDestroy())
        {
            TrapSpawner.Instance.Destroy(this.transform);
        }

        if (this.IsSeePlayer == false)
        {
            if (this.PlayerInSight())
            {
                this.IsSeePlayer = true;
                this.IsAttacking = true;
            }
        }

    }

    private bool PlayerInSight()
    {
        float xSize = this.CapsuleCollider.bounds.size.x * this._colliderWidth;
        float ySize = this.CapsuleCollider.bounds.size.y * this._colliderHeight;

        RaycastHit2D hit = Physics2D.BoxCast(
            new Vector2(this.CapsuleCollider.bounds.center.x - (xSize / 2)
            , this.CapsuleCollider.bounds.center.y + this._patrolDirection * (ySize /2)),
            new Vector2(xSize, ySize)
            , 0, Vector2.left, 0, this._playerLayerMask);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float xSize = this.CapsuleCollider.bounds.size.x * this._colliderWidth;
        float ySize = this.CapsuleCollider.bounds.size.y * this._colliderHeight;
        Gizmos.DrawWireCube(
            new Vector2(this.CapsuleCollider.bounds.center.x - (xSize / 2)
            , this.CapsuleCollider.bounds.center.y + this._patrolDirection * (ySize / 2)),
            new Vector2(xSize, ySize));
    }

    private void Attack()
    {
        this.Animator.SetTrigger("isOn");

        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.CalculateTheRotationAngleTowardsThePlayer();

        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.LaserGunBullet, spawnPosition, spawnRotation);
        poolObject.gameObject.SetActive(true);
    }

    private Quaternion CalculateTheRotationAngleTowardsThePlayer()
    {
        Vector2 playerPosition = GameMode.Instance.Player.transform.position;
        playerPosition.x += 10;
        Vector2 currentPosition = this.transform.position;

        Vector2 diff = playerPosition - currentPosition;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, rot_z);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }
}
