using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : RyoMonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Transform[] _laserGenerators_Transform = new Transform[2];
    [SerializeField] private Animator[] _laserGenerators_Animator = new Animator[2];
    [SerializeField] private Animator _laserBeam;
    [SerializeField] private SpriteRenderer _laserBeam_SR;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private float _lifeTime = 1f;
    [SerializeField] private float _xAxisStart = 15f;
    [SerializeField] private float _xAxisEnd = 10f;
    [SerializeField] private bool _isOn = false;
    [SerializeField] private bool _isCausedDamage = false;
    [SerializeField] private float _damage = 100f;
    public BoxCollider2D BoxCollider
    {
        get { return this._boxCollider; }
        private set { this._boxCollider = value; }
    }
    public Transform[] LaserGenerators_Transform
    {
        get { return this._laserGenerators_Transform; }
        private set { this._laserGenerators_Transform = value; }
    }
    public Animator[] LaserGenerators_Animator
    {
        get { return this._laserGenerators_Animator; }
        private set { this._laserGenerators_Animator = value; }
    }
    public Animator LaserBeam
    {
        get { return this._laserBeam; }
        private set { this._laserBeam = value; }
    }
    public SpriteRenderer LaserBeam_SR
    {
        get { return this._laserBeam_SR; }
        private set { this._laserBeam_SR = value; }
    }
    public LayerMask PlayerLayer
    {
        get { return this._playerLayer; }
        private set { this._playerLayer = value; }
    }
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }
    public bool IsCausedDamage
    {
        get { return this._isCausedDamage; }
        private set { this._isCausedDamage = value; }
    }
    public float DelayTime => this._delayTime;
    public float FifeTime => this._lifeTime;
    public float X_AxisStart => this._xAxisStart;
    public float X_AxisEnd => this._xAxisEnd;
    public float Damage => this._damage;

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadBoxCollider();
        this.LoadAnimator_LaserGenerator();
        this.LoadTransform_LaserGenerator();
        this.LoadAnimator_LaserBeam();
    }

    private void LoadBoxCollider()
    {
        if (this.BoxCollider != null) return;
        this.BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void LoadTransform_LaserGenerator()
    {
        if (this.LaserGenerators_Transform[0] != null && this.LaserGenerators_Transform[1] != null) return;
        this.LaserGenerators_Transform[0] = this.transform.Find("LaserGenerator_First");
        this.LaserGenerators_Transform[1] = this.transform.Find("LaserGenerator_Second");
    }

    private void LoadAnimator_LaserBeam()
    {
        if (this.LaserGenerators_Animator[0] != null && this.LaserGenerators_Animator[1] != null) return;
        this.LaserGenerators_Animator[0] = LaserGenerators_Transform[0]?.GetComponent<Animator>();
        this.LaserGenerators_Animator[1] = LaserGenerators_Transform[1]?.GetComponent<Animator>();
    }

    private void LoadAnimator_LaserGenerator()
    {
        if (this.LaserBeam != null) return;
        Transform laserBeam = this.transform.Find("LaserBeam");

        if (laserBeam == null) return;
        this.LaserBeam_SR = laserBeam.GetComponent<SpriteRenderer>();
        this.LaserBeam = laserBeam.GetComponent<Animator>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();
        this.IsOn = false;
        this.IsCausedDamage = false;
        this.PlayerLayer = LayerMask.GetMask("PlayerLayer");
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.SetLaserStatus(false);

        StartCoroutine(this.LaserGeneratorReady());
    }

    private void Update()
    {
        if (this.IsOn)
        {
            this.CheckForCollisionWithPlayer();
        }
    }

    private IEnumerator LaserGeneratorReady()
    {
        float timeCounter = 0;
        while (timeCounter < this.DelayTime)
        {
            timeCounter += Time.deltaTime;

            LaserGenerators_Transform[0].localPosition = new Vector3(
                Mathf.Lerp(-this.X_AxisStart, -this.X_AxisEnd, timeCounter / this.DelayTime), 0, 0);
            
            LaserGenerators_Transform[1].localPosition = new Vector3(
                Mathf.Lerp(this.X_AxisStart, this.X_AxisEnd, timeCounter / this.DelayTime),0, 0);

            yield return null;
        }

        StartCoroutine(this.ReadyToShoot());
    }

    private IEnumerator ReadyToShoot()
    {
        float timeCounter = 0;
        this.SetLaserStatus(true);

        while (timeCounter < this.DelayTime)
        {
            timeCounter += Time.deltaTime;

            this.LaserBeam_SR.color = new Color(1, 1, 1, Mathf.Lerp(0, 0.4f, timeCounter / this.DelayTime));

            yield return null;
        }

        timeCounter = 0;
        while (timeCounter < this.DelayTime)
        {
            timeCounter += Time.deltaTime;

            this.LaserBeam_SR.color = new Color(1, 1, 1, Mathf.Lerp(0.4f, 0.1f, timeCounter / this.DelayTime));

            yield return null;
        }
        
        timeCounter = 0;
        while (timeCounter < this.DelayTime / 10)
        {
            timeCounter += Time.deltaTime;

            this.LaserBeam_SR.color = new Color(1, 1, 1, Mathf.Lerp(0.1f, 1f, timeCounter / (this.DelayTime / 10)));

            yield return null;
        }

        this.IsOn = true;
        this.LaserBeam_SR.color = new Color(1, 1, 1, 1);

        yield return new WaitForSecondsRealtime(this.FifeTime);

        this.IsOn = false;
        StartCoroutine(this.TimerForDestroy());

    }

    private IEnumerator TimerForDestroy()
    {
        this.SetLaserStatus(false);

        float timeCounter = 0;
        while (timeCounter < this.DelayTime)
        {
            timeCounter += Time.deltaTime;

            LaserGenerators_Transform[0].localPosition = new Vector3(
                Mathf.Lerp(-this.X_AxisEnd, - this.X_AxisStart, timeCounter / this.DelayTime), 0, 0);

            LaserGenerators_Transform[1].localPosition = new Vector3(
                Mathf.Lerp(this.X_AxisEnd, this.X_AxisStart, timeCounter / this.DelayTime), 0, 0);

            yield return null;
        }

        TrapSpawner.Instance.Destroy(this.transform);
    }

    private void SetLaserStatus(bool status)
    {
        if (this.LaserGenerators_Animator[0] == null
            || this.LaserGenerators_Animator[1] == null
            || this.LaserBeam == null) return;

        this.LaserGenerators_Animator[0].SetBool("isOn", status);
        this.LaserGenerators_Animator[1].SetBool("isOn", status);
        this.LaserBeam.SetBool("isOn", status);
    }

    private void CheckForCollisionWithPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, Vector2.one, 0, this.PlayerLayer);
        if (hit.collider != null && this.IsCausedDamage == false)
        {
            I_Damageable damageable = hit.collider.GetComponent<I_Damageable>();
            if (damageable != null)
            {
                this.IsCausedDamage = true;
                this.IsOn = false;
                damageable.TakeDamage(this.Damage);
            }

        }
    }

}
