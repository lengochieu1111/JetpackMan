using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : RyoMonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _crosshair;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _targetTrackingTime = 3f;
    [SerializeField] private float _targetTrackingSpeed = 0.01f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private bool _isRushingOut = false;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private Coroutine _rushingOutCoroutine;
    private float _timeCounter;
    private float _currentSpeed;
    public Collider2D Collider
    {
        get { return this._collider; }
        private set { this._collider = value; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return this._rigidbody; }
        private set { this._rigidbody = value; }
    }
    public Transform Target
    {
        get { return this._target; }
        private set { this._target = value; }
    }
    public Transform Crosshair
    {
        get { return this._crosshair; }
        private set { this._crosshair = value; }
    }
    public float CurrentSpeed
    {
        get { return this._currentSpeed; }
        private set { this._currentSpeed = value; }
    }
    public bool IsRushingOut
    {
        get { return this._isRushingOut; }
        private set { this._isRushingOut = value; }
    }
    public float Speed => this._speed;
    public float Damage => this._damage;
    public float LifeTime => this._lifeTime;
    public float TargetTrackingTime => this._targetTrackingTime; 
    public float TargetTrackingSpeed => this._targetTrackingSpeed; 


    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadRigidbody();
        this.LoadTarget();
        this.LoadCrosshair();
    }

    private void LoadCrosshair()
    {
        if (this.Crosshair != null) return;
        this.Crosshair = this.transform.GetChild(0);
    }

    private void LoadCollider()
    {
        if (this.Collider != null) return;
        this.Collider = GetComponent<Collider2D>();
    }
    
    private void LoadRigidbody()
    {
        if (this.Rigidbody != null) return;
        this.Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void LoadTarget()
    {
        if (this.Target != null) return;
        this.Target = GameMode.Instance.Player.transform;
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.Collider != null)
        {
            this.Collider.isTrigger = true;
        }
        
        if (this.Rigidbody != null)
        {
            this.Rigidbody.gravityScale = 0;
            this.Rigidbody.freezeRotation = true;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            if (damageable.TakeDamage(this.Damage))
            {
                TrapSpawner.Instance.Destroy(this.transform);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this._timeCounter = 0;
        this.IsRushingOut = false;
        this.CurrentSpeed = this.Speed;
        this._rushingOutCoroutine = StartCoroutine(ReadyToRushOut());

        this.Crosshair.gameObject.SetActive(true);
        float distanceToPlayer = this.transform.position.x - this.Target.position.x;
        this.Crosshair.transform.localPosition = new Vector3(-distanceToPlayer, 0, 0);

    }

    private void Update()
    {
        if (this.IsRushingOut)
        {
            this.RushingOut();
            this.TimerForDestroy();
        }
        else
        {
            this.DetermineTargetLocation();
        }
    }

    private void RushingOut()
    {
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.Rigidbody.velocity = new Vector2(-this.CurrentSpeed, this.Rigidbody.velocity.y);
    }
    
    private void DetermineTargetLocation()
    {
        float yAxisPosition = this.transform.position.y;
        float yAxisTargetPosition = this.Target.position.y;
        float newY_AxisPosition = Mathf.Lerp(yAxisPosition, yAxisTargetPosition, this.TargetTrackingSpeed);

        this.transform.position = new Vector3(this.transform.position.x, newY_AxisPosition, this.transform.position.z);
    }

    private IEnumerator ReadyToRushOut()
    {
        yield return new WaitForSecondsRealtime(this.TargetTrackingTime);
        this.Crosshair.gameObject.SetActive(false);
        this.IsRushingOut = true;
    }

    private void TimerForDestroy()
    {
        this._timeCounter += Time.deltaTime;

        if (this._timeCounter > this.LifeTime)
        {
            TrapSpawner.Instance.Destroy(this.transform);
        }
    }


}
