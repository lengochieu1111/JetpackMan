using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserTwo : RyoMonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private Transform _leftLaser, _rightLaser;
    [SerializeField] private Animator[] _animators;

    [SerializeField] private float[] _xAxisStart = { -20, 20 };
    [SerializeField] private float[] _xAxisEnd = { -9, 9 };

    [SerializeField] private float _laserActiveTime = 2f;
    [SerializeField] private float _laserAppearTime = 2f;
    [SerializeField] private float _damage = 100f;

    private Coroutine _onActiveStatusCoroutine;
    private Coroutine _waitToTurnOffActiveStatusCoroutine;
    private Coroutine _offActiveStatusCoroutine;
    public CapsuleCollider2D CapsuleCollider => _capsuleCollider;
    public Animator[] Animators => _animators;
    public Transform LeftLaser => _leftLaser;
    public Transform RightLaser => _rightLaser;
    public float Damage => _damage;
    public float LaserActiveTime => _laserActiveTime;
    public float LaserAppearTime => _laserAppearTime;
    public float[] X_AxisStart => _xAxisStart;
    public float[] X_AxisEnd => _xAxisEnd;

    #region Component & Value
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLaserTransform();
        this.LoadAnimators();
        this.LoadCapsuleCollider();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    
    private void LoadLaserTransform()
    {
        if (this._leftLaser == null)
            this._leftLaser = this.transform.Find("Left");

        if (this._rightLaser == null)
            this._rightLaser = this.transform.Find("Right");
    }

    private void LoadAnimators()
    {
        if (this._animators.Count() > 0) return;
        this._animators = this.GetComponentsInChildren<Animator>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this.CapsuleCollider.isTrigger = true;
        this.CapsuleCollider.enabled = false;

        this.LeftLaser.transform.localPosition = new Vector2(this.X_AxisStart[0], this.transform.position.y);
        this.RightLaser.transform.localPosition = new Vector2(this.X_AxisStart[1], this.transform.position.y);
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this._onActiveStatusCoroutine = StartCoroutine(this.OnActiveStatus());
    }

    private IEnumerator OnActiveStatus()
    {
        float timeCounter = 0f;

        while (timeCounter < this.LaserAppearTime)
        {
            timeCounter += Time.deltaTime;

            this.RightLaser.localPosition = new Vector2(Mathf.Lerp(this.X_AxisStart[1], this.X_AxisEnd[1], timeCounter / this.LaserAppearTime), 0);
            this.LeftLaser.localPosition = new Vector2(Mathf.Lerp(this.X_AxisStart[0], this.X_AxisEnd[0], timeCounter / this.LaserAppearTime), 0);

            yield return null;
        }

        this.SetActiveStatus_Animator(true);
        this._waitToTurnOffActiveStatusCoroutine = StartCoroutine(this.WaitToTurnOffActiveStatus());
    }

    private IEnumerator WaitToTurnOffActiveStatus()
    {
        yield return new WaitForSecondsRealtime(this.LaserActiveTime);

        this.SetActiveStatus_Animator(false);
        this._offActiveStatusCoroutine = StartCoroutine(this.OffActiveStatus());
    }

    private IEnumerator OffActiveStatus()
    {
        float timeCounter = 0f;

        while (timeCounter < this.LaserAppearTime)
        {
            timeCounter += Time.deltaTime;

            this.RightLaser.localPosition = new Vector2(Mathf.Lerp(this.X_AxisEnd[1], this.X_AxisStart[1], timeCounter / this.LaserAppearTime), 0);
            this.LeftLaser.localPosition = new Vector2(Mathf.Lerp(this.X_AxisEnd[0], this.X_AxisStart[0], timeCounter / this.LaserAppearTime), 0);

            yield return null;
        }

        TrapSpawner.Instance.Destroy(this.transform);
    }

    private void SetActiveStatus_Animator(bool active)
    {
        foreach (Animator animator in this.Animators)
            animator.SetBool("isOn", active);
    }

    public void SetActiveStatus_CapsuleCollider(bool active)
    {
        this.CapsuleCollider.enabled = active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_Damageable damageable = collision.GetComponent<I_Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(this.Damage);
        }
    }

}
