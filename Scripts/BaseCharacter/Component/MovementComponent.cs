using MVCS.Architecture.BaseCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementComponent : BaseCharacterAbstract
{
    #region PROPERTTY
    [Header("Component")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CharacterSO _characterSO;
    [SerializeField] private BaseCharacterController _controller;

    [Header("Stat")]
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isFlying;
    [SerializeField] private bool _isRushingUpHigh;
    [SerializeField] private bool _isAccelerating;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _airRunningSpeed;

    [SerializeField] private float _timeToFlyUp = 0.12f;
    [SerializeField] private float _accelerationTime = 4f;
    [SerializeField] private float _nextAccelerationTime = 20f;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _timeCounter;
    private Coroutine _flyCoroutine;

    public Rigidbody2D Rigidbody => _rigidbody;
    public BaseCharacterController Controller => _controller;
    public CharacterSO CharacterSO
    {
        get { return this._characterSO; }
        private set 
        { 
            if (value != null)
            {
                this.RunSpeed = value.RunSpeed;
                this.AirRunningSpeed = value.AirRunningSpeed;
            }

            this._characterSO = value; 
        }
    }

    public bool IsRunning
    {
        get { return this._isRunning; }
        private set
        {
            if (value)
            {
                this.MovementSpeed = this.RunSpeed;
                this.Run();
            }
            else
            {
                this.MovementSpeed = 0;
                this.Idle();
            }

            this._isRunning = value;
        }
    }

    public bool IsFlying
    {
        get { return this._isFlying; }
        private set
        {
            if (value)
            {
                this.MovementSpeed = this.AirRunningSpeed;
                this.Run();
                this.Fly();
            }

            this._isFlying = value;
        }
    }
    
    public bool IsAccelerating
    {
        get { return this._isAccelerating; }
        private set { this._isAccelerating = value; }
    }

    public bool IsRushingUpHigh
    {
        get { return this._isRushingUpHigh; }
        private set { this._isRushingUpHigh = value; }
    }

    public float MovementSpeed
    {
        get { return _movementSpeed; }
        private set { this._movementSpeed = value; }
    }

    public float RunSpeed
    {
        get { return _runSpeed; }
        private set { this._runSpeed = value; }
    }

    public float AirRunningSpeed
    {
        get { return _airRunningSpeed; }
        private set { this._airRunningSpeed = value; }
    }

    public float TimeToFlyUp => this._timeToFlyUp; 
    public float AccelerationTime => this._accelerationTime; 
    public float NextAccelerationTime => this._nextAccelerationTime; 
    #endregion

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadController();
        this.LoadCharacterSO();
        this.LoadRigidbody();
    }

    private void LoadCharacterSO()
    {
        this.CharacterSO = this.Character?.Model?.CharacterSO;
    }
    
    private void LoadController()
    {
        if (this._controller != null) return;
        this._controller = this.Character?.Controller;
    }

    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;

        this._rigidbody = this.Character?.Rigidbody;
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this._timeCounter = 0;
    }

    private void Update()
    {
        if (this.Controller.IsDead) return;

        if (this.IsAccelerating == false)
        {
            this._timeCounter += Time.deltaTime;

            if (this._timeCounter > this.NextAccelerationTime)
            {
                this._timeCounter = 0;
                this.IsAccelerating = true;
            }
        }
        else
        {
            if (this._timeCounter < this.AccelerationTime)
            {
                this._timeCounter += Time.deltaTime;
                this.MovementSpeed += Time.deltaTime;
                this.RunSpeed += Time.deltaTime;
                this.AirRunningSpeed += Time.deltaTime;
            }
            else
            {
                this._timeCounter = 0;
                this.IsAccelerating = false;
            }
        }
    }

    #region Run
    public void RequestRun(bool isRunning)
    {
        this.IsRunning = isRunning;
        this.IsFlying = false;
        this.IsRushingUpHigh = false;
    }

    private void Run()
    {
        if (this.Rigidbody == null) return;
        this.Rigidbody.velocity = new Vector2(this.MovementSpeed, this.Rigidbody.velocity.y);
    }
    #endregion

    private void Idle()
    {
        if (this.Rigidbody == null) return;
        this.Rigidbody.velocity = Vector2.zero;
    }

    #region Fly
    public void RequestFly(bool isFlying)
    {
        if (isFlying)
        {
            this.IsFlying = isFlying;
            // this.IsRunning = false;
        }

        this.IsRushingUpHigh = isFlying;
    }

    private void Fly()
    {
        if (this.Rigidbody == null) return;

        if (this.Controller.IsOnGround)
        {
            this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x, 3);
        }
        else
        {
            this._flyCoroutine = StartCoroutine(this.FlyDelay());
        }

    }

    private IEnumerator FlyDelay()
    {
        float elapsedTime = 0;
        float yVelocity = this.Rigidbody.velocity.y;

        while (elapsedTime < this.TimeToFlyUp)
        {
            elapsedTime += Time.deltaTime;

            this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x,
                Mathf.Lerp(yVelocity, 5, elapsedTime / this.TimeToFlyUp));

            yield return null;
        }

        // this.IsRushingUpHigh = true;
    }

    #endregion

    public void RequestDead()
    {
        this.IsRunning = false;
        this.IsFlying = false;
        this.IsRushingUpHigh = false;
    }

}
