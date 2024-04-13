using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBullet : RyoMonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _warning;
    [SerializeField] private SpriteRenderer _sprite_Warning;
    [SerializeField] private SpriteRenderer _sprite_LightBullet;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _targetTrackingTime = 3f;
    [SerializeField] private float _targetTrackingSpeed = 0.01f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private bool _isRushingOut = false;
    [SerializeField] private Collider2D _collider;
    private Coroutine _rushingOutCoroutine;
    private float _currentSpeed;
    public Collider2D Collider => this._collider;
    public Transform Target => this._target;
    public Transform Warning => this._warning;
    public SpriteRenderer Sprite_Warning => _sprite_Warning;
    public SpriteRenderer Sprite_LightBullet => _sprite_LightBullet;
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
        this.LoadTarget();
        this.LoadWarning();
        this.LoadSpriteWarning();
        this.LoadSpriteLightBullet();
    }

    private void LoadWarning()
    {
        if (this._warning != null) return;
        this._warning = this.transform.GetChild(0);
    }
    private void LoadSpriteWarning()
    {
        if (this._sprite_Warning != null) return;
        this._sprite_Warning = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void LoadSpriteLightBullet()
    {
        if (this._sprite_LightBullet != null) return;
        this._sprite_LightBullet = this.GetComponent<SpriteRenderer>();
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

        this.IsRushingOut = false;
        this.CurrentSpeed = this.Speed;

        this.Warning.gameObject.SetActive(true);
        this._rushingOutCoroutine = StartCoroutine(ReadyToRushOut());
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
        this.Warning.gameObject.SetActive(false);
        this.IsRushingOut = true;
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = true;
        bool canDestroy_2 = this.transform.position.x < Camera.main.transform.position.x;

        if (this.Sprite_LightBullet.isVisible || this.Sprite_Warning.isVisible)
        {
            canDestroy_1 = false;
        }

        return canDestroy_1 & canDestroy_2;
    }

}
