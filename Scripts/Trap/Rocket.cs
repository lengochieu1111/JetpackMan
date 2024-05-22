using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : RyoMonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _warning;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite_Warning;
    [SerializeField] private SpriteRenderer _sprite_Rocket;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _targetTrackingTime = 0.8f;
    [SerializeField] private float _targetTrackingSpeed = 0.01f;
    [SerializeField] private bool _isRushingOut = false;
    [SerializeField] private bool _isReadyToAttack = false;
    [SerializeField] float _amplitude = 10f; // Độ lắc lư lên xuống
    [SerializeField] float _frequency = 10f; // Tần số lắc lư
    private float _timeCounter;
    private Coroutine _destroyCoroutine;
    private Coroutine _rushingOutCoroutine;
    private float _currentSpeed;
    public Collider2D Collider => this._collider;
    public Transform Target => this._target;
    public Transform Warning => this._warning;
    public Animator Animator => this._animator;
    public SpriteRenderer Sprite_Warning => _sprite_Warning;
    public SpriteRenderer Sprite_Rocket => _sprite_Rocket;
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
    public bool IsReadyToAttack
    {
        get { return this._isReadyToAttack; }
        private set { this._isReadyToAttack = value; }
    }
    public float Speed => this._speed;
    public float Damage => this._damage;
    public float TargetTrackingTime => this._targetTrackingTime;
    public float TargetTrackingSpeed
    {
        get { return this._targetTrackingSpeed; }
        private set { this._targetTrackingSpeed = value; }
    }

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadAnimation();
        this.LoadTarget();
        this.LoadWarning();
        this.LoadSpriteWarning();
        this.LoadSpriteRocket();
    }

    private void LoadWarning()
    {
        if (this._warning != null) return;
        this._warning = this.transform.GetChild(0);
    }
    private void LoadAnimation()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
    }
    private void LoadSpriteWarning()
    {
        if (this._sprite_Warning != null) return;
        this._sprite_Warning = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void LoadSpriteRocket()
    {
        if (this._sprite_Rocket != null) return;
        this._sprite_Rocket = this.GetComponent<SpriteRenderer>();
    }

    private void LoadCollider()
    {
        if (this._collider != null) return;
        this._collider = GetComponent<Collider2D>();
    }
    private void LoadTarget()
    {
        if (this._target != null) return;
        this._target = GameMode.Instance.Player.transform;
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.Collider != null)
        {
            this.Collider.isTrigger = true;
        }

        this.Warning?.gameObject.SetActive(true);
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 1f);

    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsRushingOut = false;
        this.IsReadyToAttack = false;
        this.CurrentSpeed = this.Speed;
        this.TargetTrackingSpeed = Random.Range(0.01f, 0.025f);
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.Animator.SetBool(AnimationString.isOn, true);
    }

    protected override void Start()
    {
        base.Start();

        this._timeCounter = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            if (damageable.TakeDamage(this.Damage))
            {
                this._destroyCoroutine = StartCoroutine(this.DestroyCoroutine());
            }
        }
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            TrapSpawner.Instance.Destroy(this.transform);
        }

        if (this.IsRushingOut)
        {
            this.RushingOut();
        }
        else
        {
            this.DetermineTargetLocation();
        }
    }

    private void RushingOut()
    {
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.transform.Translate(Vector2.left * this.CurrentSpeed * Time.deltaTime);

        this._timeCounter += Time.deltaTime;
        float newRotationZ = Mathf.Sin(this._timeCounter * this._frequency) * this._amplitude;
        this.transform.rotation = Quaternion.Euler(0, 0, newRotationZ);
    }

    private void DetermineTargetLocation()
    {
        float yAxisPosition = this.transform.position.y;
        float yAxisTargetPosition = this.Target.position.y;
        float newY_AxisPosition = Mathf.Lerp(yAxisPosition, yAxisTargetPosition, this.TargetTrackingSpeed);

        this.transform.position = new Vector3(this.transform.position.x, newY_AxisPosition, this.transform.position.z);

        float distanceToPlayer = Random.Range(1f, 3f);
        if (Mathf.Abs(yAxisPosition - yAxisTargetPosition) < distanceToPlayer && this.IsReadyToAttack == false)
        {
            this.IsReadyToAttack = true;
            this._rushingOutCoroutine = StartCoroutine(this.ReadyToRushOut());
        }

    }

    private IEnumerator ReadyToRushOut()
    {
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 0.2f);
        yield return new WaitForSecondsRealtime(this.TargetTrackingTime / 4.0f);
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 1f);

        yield return new WaitForSecondsRealtime(this.TargetTrackingTime / 4.0f);
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 0.2f);

        yield return new WaitForSecondsRealtime(this.TargetTrackingTime / 4.0f);
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 1f);

        yield return new WaitForSecondsRealtime(this.TargetTrackingTime / 4.0f);
        this.Sprite_Warning.color = new Color(0.7f, 0f, 0f, 0.2f);

        this.IsRushingOut = true;
        this.Warning.gameObject.SetActive(false);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = true;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        if (this.Sprite_Rocket.isVisible || this.Sprite_Warning.isVisible)
        {
            canDestroy_1 = false;
        }

        return canDestroy_1 & canDestroy_2;
    }

    private IEnumerator DestroyCoroutine()
    {
        this.Animator.SetBool("isOn", false);
        yield return new WaitForSecondsRealtime(2f);
        TrapSpawner.Instance.Destroy(this.transform);
    }

}
