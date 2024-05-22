using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UFO : RyoMonoBehaviour, I_Damageable
{
    [SerializeField] private Image _healthbar_Image;
    [SerializeField] private Animator _animator;
    [SerializeField] private Laser_UFO _laser_UFO;
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private bool _isAttacking;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private float _startAttackTime = 7f;
    [SerializeField] private float _nextAttackTime = 5;
    [SerializeField] private float _health = 8000;
    [SerializeField] private float _healthMax = 8000;
    private Coroutine _startAttackCoroutine;
    private Coroutine _nextAttackCoroutine;

    public CapsuleCollider2D CapsuleCollider => this._capsuleCollider;
    public Animator Animator => this._animator;
    public Laser_UFO Laser_UFO => this._laser_UFO;
    public Image Healthbar_Image => this._healthbar_Image;
    public bool IsAttacking
    {
        get { return this._isAttacking; }
        private set
        {
            if (value)
            {
                this._startAttackCoroutine = StartCoroutine(this.StartAttack_Coroutine());
            }
            else
            {
                this.EndAttack();
            }

            this._isAttacking = value;
        }
    }
    public float NextAttackTime => _nextAttackTime;
    public float StartAttackTime => _startAttackTime;
    public float Health
    {
        get { return _health; }
        private set 
        {
            this._health = value;
            this.UpdateHealthbar();
        }
    }
    public float HealthMax
    {
        get { return _healthMax; }
        private set { _healthMax = value; }
    }
    [Header("Attack One")]
    [SerializeField] private bool _isAttacking_One;
    [SerializeField] private float[] _xAxisSpawnMissile_AttackOne = { -1, -3, -5 };
    private int _attackCounter_AttackOne;
    private Coroutine _attackOneCoroutine;
    public bool IsAttacking_One
    {
        get { return this._isAttacking_One; }
        private set
        {
            if (value)
            {
                this.Animator?.SetTrigger(AnimationString.isAttackOne);
            }

            this.Animator.SetBool(AnimationString.isAttacking_One, value);
            this._isAttacking_One = value;
        }
    }
    public float[] X_AxisSpawnMissile_AttackOne => _xAxisSpawnMissile_AttackOne;

    [Header("Attack Two")]
    [SerializeField] private bool _isAttacking_Two;
    [SerializeField] private int _maximuNumberOfMissile_AttackTwo = 3;
    [SerializeField] private float _missileSpawnTime_AttackTwo = 3f;
    [SerializeField] private float _laserAttackTime_AttackTwo = 2f;
    private int _missileCounter_AttackTwo;
    private float _missileSpawnTimer_AttackTwo;
    private Coroutine _laserAttackCoroutine;
    public bool IsAttacking_Two
    {
        get { return this._isAttacking_Two; }
        private set
        { this._isAttacking_Two = value; }
    }

    public int MaximuNumberOfMissile_AttackTwo => _maximuNumberOfMissile_AttackTwo;
    public float MissileSpawnTime_AttackTwo => _missileSpawnTime_AttackTwo;
    public float LaserAttackTime_AttackTwo => _laserAttackTime_AttackTwo;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCapsuleCollider();
        this.LoadAnimation();
        this.LoadLaser_UFO();
        this.LoadHealthbar_Image();
    }

    private void LoadAnimation()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
    }
    
    private void LoadLaser_UFO()
    {
        if (this._laser_UFO != null) return;
        this._laser_UFO = GetComponentInChildren<Laser_UFO>(true);
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    
    private void LoadHealthbar_Image()
    {
        if (this._healthbar_Image != null) return;
        Transform healthbar = this.FindChildByName(this.transform, "Healthbar_Image");
        this._healthbar_Image = healthbar?.GetComponent<Image>();
    }

    private Transform FindChildByName(Transform parrent, string childName)
    {
        Transform childObject = parrent.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrent)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }
    }

    #endregion

    protected override void SetupComponents()
    {
        base.SetupComponents();

        if (this.CapsuleCollider != null)
        {
            this.CapsuleCollider.isTrigger = true;
        }

        this.Laser_UFO.gameObject.SetActive(false);

    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._playerLayerMask = LayerMask.GetMask(LayerMaskString.PlayerLayer);

        this._health = this._healthMax;
        this._isAttacking = false;
        this._isAttacking_One = false;
        this._isAttacking_Two = false;

        this._attackCounter_AttackOne = 0;
        this._missileCounter_AttackTwo = 0;
        this._missileSpawnTimer_AttackTwo = 0;

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.IsAttacking = true;
        this.Health = this.HealthMax;
    }

    private void Update()
    {
        if (this.IsAttacking_Two)
        {
            this.AttackTwoProcess();
        }

    }

    private IEnumerator StartAttack_Coroutine()
    {
        yield return new WaitForSeconds(this.StartAttackTime);
        this.StartAttack();
    }

    private void StartAttack()
    {
        this.IsAttacking_One = true;
        this.IsAttacking_Two = false;
    }

    private IEnumerator NextAttack(bool isAttackOne)
    {
        yield return new WaitForSeconds(this.NextAttackTime);

        this.IsAttacking_One = isAttackOne;
        this.IsAttacking_Two = !isAttackOne;
    }

    private void EndAttack()
    {
        this.IsAttacking_One = false;
        this.IsAttacking_Two = false;
    }

    #region AttackOne
    public void RequestAttackOne()
    {
        if (this._attackCounter_AttackOne >= this.X_AxisSpawnMissile_AttackOne.Length)
        {
            this.IsAttacking_One = false;
            this._attackCounter_AttackOne = 0;
            this._nextAttackCoroutine = StartCoroutine(this.NextAttack(false));
        }
        else
        {
            if(this._attackCounter_AttackOne == 0)
            {
                this.AttackOne_First();
            }            
            else if(this._attackCounter_AttackOne == 1)
            {
                this.AttackOne_Second();
            }
            else
            {
                this.AttackOne_Third();
            }

            this._attackCounter_AttackOne++;
            this._attackOneCoroutine = StartCoroutine(this.NetxAttackOne());
        }
    }

    private IEnumerator NetxAttackOne()
    {
        yield return new WaitForSeconds(3f);
        this.IsAttacking_One = true;
    }

    private void AttackOne_First()
    {
        this.IsAttacking_One = true;

        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.transform.rotation;

        spawnPosition.y = this.X_AxisSpawnMissile_AttackOne[2];
        this.SpawnMissile_Default(spawnPosition, spawnRotation);

        this.IsAttacking_One = false;
    }

    private void AttackOne_Second()
    {
        this.IsAttacking_One = true;

        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.transform.rotation;

        spawnPosition.y = this.X_AxisSpawnMissile_AttackOne[1];
        this.SpawnMissile_Default(spawnPosition, spawnRotation);

        spawnPosition.y = this.X_AxisSpawnMissile_AttackOne[2];
        this.SpawnMissile_Default(spawnPosition, spawnRotation);

        this.IsAttacking_One = false;
    }

    private void AttackOne_Third()
    {
        this.IsAttacking_One = true;

        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.transform.rotation;

        for (int i = 0; i < this.X_AxisSpawnMissile_AttackOne.Length; i++)
        {
            spawnPosition.y = this.X_AxisSpawnMissile_AttackOne[i];
            this.SpawnMissile_Default(spawnPosition, spawnRotation);
        }

        this.IsAttacking_One = false;
    }

    private void SpawnMissile_Default(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Transform poolObject2 = TrapSpawner.Instance.Spawn(TrapSpawner.Missile_Default, spawnPosition, spawnRotation);
        poolObject2.gameObject.SetActive(true);
    }

    #endregion

    /*
     * 
     */
    private void AttackTwoProcess()
    {
        if (this._missileCounter_AttackTwo >= this.MaximuNumberOfMissile_AttackTwo)
        {
            this.IsAttacking_Two = false;
            this._missileCounter_AttackTwo = 0;
            this.Animator?.SetBool(AnimationString.isAttacking_One, false);
            this._laserAttackCoroutine = StartCoroutine(this.AttackTwo_Laser());
        }

        if (this._missileSpawnTimer_AttackTwo >= this.MissileSpawnTime_AttackTwo)
        {
            this._missileSpawnTimer_AttackTwo = 0;
            this._missileCounter_AttackTwo++;
            this.StartAttackTwo_Missile();
        }
        else
        {
            this._missileSpawnTimer_AttackTwo += Time.deltaTime;
        }
    }

    private void StartAttackTwo_Missile()
    {
        this.Animator?.SetTrigger(AnimationString.isAttackOne);
        this.Animator?.SetBool(AnimationString.isAttacking_One, true);
    }

    public void EndAttackTwo_Missile()
    {
        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.CalculateTheRotationAngleTowardsThePlayer();
        this.SpawnMissile_Guided(spawnPosition, spawnRotation);

        this.Animator?.SetBool(AnimationString.isAttacking_One, false);
    }

    private IEnumerator AttackTwo_Laser()
    {
        this.Animator?.SetTrigger(AnimationString.isAttackTwo);
        this.Animator?.SetBool(AnimationString.isAttacking_Two, true);

        this.Laser_UFO?.gameObject.SetActive(true);
        this.Laser_UFO?.SetActiveStatus_Animator(true);

        yield return new WaitForSeconds(this.LaserAttackTime_AttackTwo);

        this.Laser_UFO?.SetActiveStatus_Animator(false);
        this.Laser_UFO?.gameObject.SetActive(false);

        this.Animator?.SetBool(AnimationString.isAttacking_Two, false);

        this._nextAttackCoroutine = StartCoroutine(this.NextAttack(true));
    }

    private Quaternion CalculateTheRotationAngleTowardsThePlayer()
    {
        Vector2 playerPosition = GameMode.Instance.Player.transform.position;
        playerPosition.x += 5;
        Vector2 currentPosition = this.transform.position;

        Vector2 diff = playerPosition - currentPosition;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, rot_z);
    }

    private void SpawnMissile_Guided(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Transform poolObject2 = TrapSpawner.Instance.Spawn(TrapSpawner.Missile_Guided, spawnPosition, spawnRotation);
        poolObject2.gameObject.SetActive(true);
    }

    public bool TakeDamage(float damage)
    {
        this.Health = Mathf.Clamp(this.Health - damage, 0, this.HealthMax);

        if (this.Health <= 0)
        {
            GameManager.Instance.SetIsWin(true);
            GameManager.Instance.SetMatchState(MatchState.ShowResult);
            this.DestroyObject();
        }

        return true;
    }

    private void DestroyObject()
    {
        TrapSpawner.Instance.Destroy(this.transform);
    }

    /*
     * 
     */

    public void SetIsAttacking(bool isAttacking)
    {
        this.IsAttacking = isAttacking;
    }

    private void UpdateHealthbar()
    {
        this.Healthbar_Image.fillAmount = this.Health * 1.0f / this.HealthMax;
    }

}
