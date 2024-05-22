using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dr_Bones : RyoMonoBehaviour, I_Damageable
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private SpriteRenderer _sprite;

    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private bool _isSeePlayer;
    [SerializeField] private bool _isFear;
    [SerializeField] private bool _isDead;
    [SerializeField] private bool _isMovingLeft;
    [SerializeField] private Vector3 _initScale;

    [SerializeField] private int _energy = 10;

    [Header("Patrol")]
    [SerializeField] private Vector2 _startingPosition;
    [SerializeField] private float _patrolRadius = 5f;
    [SerializeField] private float _currentPatrolSpeed = 1.8f;
    [SerializeField] private float _currentSpeedWhenFear = 3f;
    [SerializeField] private float[] _patrolSpeed = { 1.2f, 2.4f };
    [SerializeField] private float[] _speedWhenFear = { 3.8f, 6.2f };
    public Rigidbody2D Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public CapsuleCollider2D CapsuleCollider => _capsuleCollider;
    public SpriteRenderer Sprite => _sprite;
    public bool IsSeePlayer
    {
        get { return this._isSeePlayer; }
        private set { this._isSeePlayer = value; }
    }
    public bool IsFear
    {
        get { return this._isFear; }
        private set 
        { 
            if (value)
            {
                this.Animator.SetTrigger(AnimationString.isFear);
            }
            this._isFear = value; 
        }
    }
    public bool IsDead
    {
        get { return this._isDead; }
        private set 
        { 
            if (value)
            {
                this.Animator.SetTrigger(AnimationString.isDead);
            }
            this._isDead = value; 
        }
    }
    public bool IsMovingLeft
    {
        get { return this._isMovingLeft; }
        private set { this._isMovingLeft = value; }
    }
    public Vector2 StartingPosition => _startingPosition; 
    public Vector3 InitScale => _initScale;
    public float PatrolRadius => _patrolRadius;
    public float CurrentPatrolSpeed => _currentPatrolSpeed;
    public float CurrentSpeedWhenFear => _currentSpeedWhenFear;
    public float[] PatrolSpeed => _patrolSpeed;
    public float[] SpeedWhenFear => _speedWhenFear;
    public int Energy => _energy;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadRigidbody();
        this.LoadSprite();
        this.LoadAnimator();
        this.LoadCapsuleCollider();
    }

    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void LoadAnimator()
    {
        if (this._animator != null) return;
        this._animator = this.GetComponent<Animator>();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = this.GetComponent<CapsuleCollider2D>();
    }
    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
    }

    #endregion

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this.Rigidbody.freezeRotation = true;
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsSeePlayer = false;
        this.IsFear = false;
        this.IsMovingLeft = false;
        this.IsDead = false;

        this.Sprite.color = new Color(1, 1, 1, 1);

        this._currentPatrolSpeed = Random.Range(this.PatrolSpeed[0], this.PatrolSpeed[1]);
        this._currentSpeedWhenFear = Random.Range(this.SpeedWhenFear[0], this.SpeedWhenFear[1]);

        this._startingPosition = this.transform.position;
        this._initScale = this.transform.localScale;
        this._playerLayerMask = LayerMask.GetMask(LayerMaskString.PlayerLayer);
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.DestroyObject();
        }

        if (this.IsDead)
        {

        }
        else
        {
            if (this.IsSeePlayer == false)
            {
                if (this.PlayerInSight())
                {
                    this.IsSeePlayer = true;
                    this.IsFear = true;
                }
                else
                    this.Patrol();
            }

            if (this.IsFear)
            {
                this.Fear();
            }

            if (this.CollideWithPlayer())
            {
                this.CollisionToDeath();
            }
        }

    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.CapsuleCollider.bounds.center,
            new Vector2(this.CapsuleCollider.bounds.size.x * 40, this.CapsuleCollider.bounds.size.y * 15)
            , 0, Vector2.left, 0, this._playerLayerMask);

        return hit.collider != null;
    }

    private void Patrol()
    {
        if (this.IsMovingLeft)
        {
            if (this.transform.position.x > this.StartingPosition.x - this.PatrolRadius)
                this.Moving(-1);
            else
                this.DirectionChange();
        }
        else
        {
            if (this.transform.position.x < this.StartingPosition.x + this.PatrolRadius)
                this.Moving(1);
            else
                this.DirectionChange();
        }
    }

    private void Moving(int direction)
    {
        this.transform.localScale = new Vector3(this.InitScale.x * direction , this.InitScale.y, this.InitScale.z);
        this.transform.position = new Vector2(this.transform.position.x + direction * this.CurrentPatrolSpeed * Time.deltaTime, this.transform.position.y);
    }

    private void Fear()
    {
        this.transform.localScale = new Vector3(this.InitScale.x , this.InitScale.y, this.InitScale.z);
        this.transform.position = new Vector2(this.transform.position.x + this.CurrentSpeedWhenFear * Time.deltaTime, this.transform.position.y);
    }

    private void DirectionChange()
    {
        this.IsMovingLeft = !this.IsMovingLeft;
    }

    private bool CollideWithPlayer()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(this.CapsuleCollider.bounds.center, this.CapsuleCollider.bounds.size, 
            CapsuleDirection2D.Vertical, 0, Vector2.left, 0, this._playerLayerMask);

        return hit.collider != null;
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;
        return canDestroy_1 & canDestroy_2;
    }

    public void DestroyObject()
    {
        this.Animator.SetTrigger(AnimationString.isRevive);
        EnemySpawner.Instance.Destroy(this.transform);
    }

    public bool TakeDamage(float damage)
    {
        if (this.IsDead) return false;
        this.CollisionToDeath();
        return true;
    }

    private void CollisionToDeath()
    {
        if (this.IsDead) return;

        GameMode.Instance.HandlePlayerPickUp_Energy(this.Energy);

        this.Rigidbody.AddForce(new Vector2(500, 100));
        this.IsSeePlayer = true;
        this.IsFear = true;
        this.IsDead = true;
    }

    public bool CanDestroyWhenDead()
    {
        float aColor = this.Sprite.color.a - Time.deltaTime;
        this.Sprite.color = new Color(1, 1, 1, aColor);
        return this.Rigidbody.velocity.x <= 0 && this.Sprite.color.a <= 0;
    }

}
