using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MissileType
{
    Default,
    Guided
}

public class Missile : RyoMonoBehaviour
{
    [SerializeField] private MissileType _missileType;
    [SerializeField] private Transform _target;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;

    [SerializeField] private bool _isRushingOut = true;
    [SerializeField] private float _damage = 100f;
    private Coroutine _destroyCoroutine;
    private float _currentSpeed;
    public Collider2D Collider => this._collider;
    public Transform Target => this._target;
    public Animator Animator => this._animator;
    public SpriteRenderer Sprite => _sprite;
    public MissileType MissileType => _missileType;
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
    public float Damage => this._damage;

    [Header("Default")]
    [SerializeField] private float _defaultSpeed = 10f;
    [SerializeField] private float _amplitude = 5f; // Độ lắc lư lên xuống
    [SerializeField] private float _frequency = 20f; // Tần số lắc lư
    private float _timeCounter;
    public float DefaultSpeed => this._defaultSpeed;

    [Header("Guided")]
    [SerializeField] private float _guidedSpeed = 20f;
    [SerializeField] private float _arcHeight = 0f;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private LayerMask _groundLayer;
    public float GuidedSpeed => this._guidedSpeed;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCollider();
        this.LoadAnimation();
        this.LoadTarget();
        this.LoadSpriteRocket();
    }

    private void LoadAnimation()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
    }

    private void LoadSpriteRocket()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
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

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsRushingOut = true;
        this._timeCounter = 0;
        this._groundLayer = LayerMask.GetMask(LayerMaskString.GroundLayer);

        if (this.MissileType == MissileType.Default)
        {
            this.CurrentSpeed = this.DefaultSpeed;
        }
        else if (this.MissileType == MissileType.Guided)
        {
            this.CurrentSpeed = this.GuidedSpeed;
        }

    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.Collider.enabled = true;
        this.Animator.SetBool(AnimationString.isOn, true);

        if (this.MissileType == MissileType.Guided)
        {
            this.LaunchRocket();
        }

    }

    private void Update()
    {
        if (this.CanDestroy() && this.IsRushingOut)
        {
            this.MissileExploded();
        }

        if (this.IsRushingOut)
        {
            if (this.MissileType == MissileType.Default)
            {
                this.RushingOut_Default();
            }
            else if (this.MissileType == MissileType.Guided)
            {
                this.RushingOut_Guided();
            }

        }

    }

    private void RushingOut_Default()
    {
        // Cập nhật tốc độ
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;
        this.transform.Translate(Vector2.left * this.CurrentSpeed * Time.deltaTime);

        // Tính toán góc quay mới trên trục Z
        this._timeCounter += Time.deltaTime;
        float newZ = Mathf.Sin(this._timeCounter * this._frequency) * this._amplitude;

        // Cập nhật góc quay 
        this.transform.rotation = Quaternion.Euler(0f, 0f, newZ);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;
        bool canDestroy_One = canDestroy_1 & canDestroy_2;

        bool canDestroy_Two = false;

        if (this.MissileType == MissileType.Guided)
        {
            RaycastHit2D hit = Physics2D.CapsuleCast(this.Collider.bounds.center,
                new Vector2(this.Collider.bounds.size.x, this.Collider.bounds.size.y * 2),
                CapsuleDirection2D.Vertical, 0, Vector2.left, 0, this._groundLayer);

            canDestroy_Two = hit.collider != null;
        }

        return canDestroy_One || canDestroy_Two;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    Gizmos.DrawWireCube(this.Collider.bounds.center,
    //        new Vector2(this.Collider.bounds.size.x, this.Collider.bounds.size.y * 2));
    //}

    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        TrapSpawner.Instance.Destroy(this.transform);
    }

    private void MissileExploded()
    {
        this.IsRushingOut = false;
        this.Animator.SetBool(AnimationString.isOn, false);
        this.Collider.enabled = false;
        this._destroyCoroutine = StartCoroutine(this.DestroyObject());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            if (damageable.TakeDamage(this.Damage))
            {
                this.MissileExploded();
            }
        }
    }

    //

    #region Guided
    private void RushingOut_Guided()
    {
        this.CurrentSpeed += Time.deltaTime * this.CurrentSpeed;

        // Di chuyển đối tượng theo quỹ đạo vòng cung
        float step = this.CurrentSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, this._targetPosition, step);

        // Tính toán chiều cao của quỹ đạo
        float peakHeight = Mathf.Abs(this._targetPosition.y - transform.position.y) + _arcHeight;

        // Tính toán chiều cao của quỹ đạo dựa trên khoảng cách hiện tại
        Vector3 peak = (transform.position + this._targetPosition) / 2;
        peak.y += peakHeight;

        // Di chuyển đối tượng theo quỹ đạo vòng cung
        transform.position = Vector3.MoveTowards(transform.position, peak, step / 2);

        // Quaternion
        this.transform.rotation = CalculateTheRotationAngleTowardsThePlayer();
    }

    private void LaunchRocket()
    {
        // Thiết lập vị trí đích
        this._targetPosition = this.Target.position;
        this._targetPosition.x += 10;
        this._targetPosition.y -= 10;

        // Tính toán thời gian di chuyển
        float timeToTarget = Vector3.Distance(transform.position, this._targetPosition) / this.CurrentSpeed;

        // Gọi hàm để đáp xuống đất sau thời gian di chuyển
        Invoke("LandRocket", timeToTarget);
    }

    private void LandRocket()
    {
        this.IsRushingOut = false;

        // Xử lí khi đối tượng đáp xuống đất
        // this.gameObject.SetActive(false);
    }

    private Quaternion CalculateTheRotationAngleTowardsThePlayer()
    {
        Vector2 playerPosition = this.Target.position;
        playerPosition.x -= 10;
        playerPosition.y -= 15;

        Vector2 currentPosition = this.transform.position;

        Vector2 diff = (Vector2)playerPosition - currentPosition;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, rot_z + 180);
    }

    #endregion

}
