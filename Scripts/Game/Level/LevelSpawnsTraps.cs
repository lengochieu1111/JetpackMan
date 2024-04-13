using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnsTraps : RyoMonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Level _level;
    public Level Level
    {
        get { return this._level; }
        private set { this._level = value; }
    }

    [Header("Trap")]
    [SerializeField] private bool _isSpawningTrap;
    [SerializeField] private float _nextTrapSpawnDistance = 50f;
    [SerializeField] private int _maximumSpawnTrap = 5;
    [SerializeField] private int _trapType;
    private float _trapSpawnDistance;
    private int _spawnCounter;
    private float _distanceCounter;
    public bool IsSpawningTrap
    {
        get { return this._isSpawningTrap; }
        private set 
        {
            if (value)
            {
                this.StartTrapSpawning();
                this.RandomTrapType();
            }
            else
                this.EndTrapSpawning();

            this._isSpawningTrap = value; 
        }
    }
    public float TrapSpawnDistance
    {
        get { return this._trapSpawnDistance; }
        private set { this._trapSpawnDistance = value; }
    }
    public float NextTrapSpawnDistance
    {
        get { return this._nextTrapSpawnDistance; }
        private set { this._nextTrapSpawnDistance = value; }
    }
    public int MaximumSpawnTrap
    {
        get { return this._maximumSpawnTrap; }
        private set { this._maximumSpawnTrap = value; }
    }
    public int TrapType
    {
        get { return this._trapType; }
        private set { this._trapType = value; }
    }

    [Header("Bolt")]
    [SerializeField] private bool _isSpawningBolt;
    public bool IsSpawningBolt
    {
        get { return this._isSpawningBolt; }
        private set { this._isSpawningBolt = value; }
    }

    [Header("Fireball")]
    [SerializeField] private bool _isSpawningFireball;
    public bool IsSpawningFireball
    {
        get { return this._isSpawningFireball; }
        private set { this._isSpawningFireball = value; }
    }

    [Header("Laser")]
    [SerializeField] private bool _isSpawningLaser;
    private List<int> _spawnedIndexs = new List<int>();
    private float[] _laserSpawn_xAxis = { -3, -1.5f, 0, 1.5f, 3, 4.5f };
    public bool IsSpawningLaser
    {
        get { return this._isSpawningLaser; }
        private set { this._isSpawningLaser = value; }
    }

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevel();
    }

    private void LoadLevel()
    {
        if (this.Level != null) return;
        this.Level = GetComponent<Level>();
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsSpawningTrap = false; 
        this.IsSpawningLaser = false;
        this.IsSpawningBolt = false;
        this.IsSpawningFireball = false;
        this.TrapSpawnDistance = this.NextTrapSpawnDistance;
    }

    private void Update()
    {
        this.RandomSpawnTrap();

        if (this.IsSpawningLaser)
        {
            this.LaserSpawnProcess();
        }
        
        if (this.IsSpawningBolt) 
        {
            this.BoltSpawnProcess();
        }
        
        if (this.IsSpawningFireball) 
        {
            this.FireballSpawnProcess();
        }

    }

    private void RandomSpawnTrap()
    {
        if (Level.Instance.DistancToStartingPoint > this.TrapSpawnDistance && this.IsSpawningTrap == false)
        {
            this.IsSpawningTrap = true;

        }
    }

    private void RandomTrapType()
    {
        int oldValue = TrapType;

        do
        {
            this.TrapType = Random.Range(0, 3);
        }
        while (oldValue == this.TrapType);

        switch (this.TrapType)
        {
            case 0:
                this.IsSpawningLaser = true;
                break;
            case 1:
                this.IsSpawningBolt = true;
                break;
            case 2:
                this.IsSpawningFireball = true;
                break;
            default:
                break;
        }
    }

    private void StartTrapSpawning()
    {
        this._distanceCounter = Level.Instance.DistancToStartingPoint;
        this._spawnCounter = 0;
    }

    private void EndTrapSpawning()
    {
        this.NextTrapSpawnDistance = Random.Range(30.0f, 60.0f);
        this.TrapSpawnDistance = Level.Instance.DistancToStartingPoint + this.NextTrapSpawnDistance;
        this._spawnCounter = 0;

        this.IsSpawningBolt = false;
        this.IsSpawningLaser = false;
        this.IsSpawningFireball = false;
    }

    private void BoltSpawnProcess()
    {
        if (this._spawnCounter > this.MaximumSpawnTrap)
        {
            this.IsSpawningTrap = false;
        }

        if (Level.Instance.DistancToStartingPoint > this._distanceCounter + Random.Range(15.0f, 40.0f))
        {
            this._distanceCounter = Level.Instance.DistancToStartingPoint;
            this.MaximumSpawnTrap = Random.Range(2, 10);
            this.BoltSpawnRandom();
        }
    }

    private void BoltSpawnRandom()
    {
        int index = Random.Range(0, TrapSpawner.Bolts.Length);
        float yAxisSpawnPosition = Random.Range(-2f, 2.5f);
        float zAxisSpawnRotation = Random.Range(0f, 360f);

        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Bolts[index], 
             new Vector3 (Level.Instance.DistancToStartingPoint + 10, yAxisSpawnPosition, 0), 
             Quaternion.Euler(0, 0, zAxisSpawnRotation)
             );

        poolObject.gameObject.SetActive(true);

        this._spawnCounter++;
    }

    private void LaserSpawnProcess()
    {
        if (Level.Instance.DistancToStartingPoint > this._distanceCounter)
        {
            this._distanceCounter = Level.Instance.DistancToStartingPoint;
            this.LaserSpawnRandom();
            this.IsSpawningTrap = false;
        }
    }

    private void LaserSpawnRandom()
    {
        this._spawnedIndexs.Clear();
        this.MaximumSpawnTrap = Random.Range(1, this._laserSpawn_xAxis.Length + 1);

        for (int i = 0; i < this.MaximumSpawnTrap; i++)
        {
            int index1 = Random.Range(0, TrapSpawner.Lasers.Length);
            int index2 = Random.Range(0, this._laserSpawn_xAxis.Length);
            
            while(this._spawnedIndexs.Contains(index2))
            {
                index2 = Random.Range(0, this._laserSpawn_xAxis.Length);

            }

            if (this._spawnedIndexs.Contains(index2) == false)
            {
                this._spawnedIndexs.Add(index2);
            }

            Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Lasers[index1],
                 CameraManager.Instance.transform.position,
                 Quaternion.identity
                 );

            poolObject.SetParent(CameraManager.Instance.transform);
            poolObject.transform.localPosition = new Vector3(0, this._laserSpawn_xAxis[index2], -5);
            poolObject.gameObject.SetActive(true);
        }

    }

    private void FireballSpawnProcess()
    {
        if (this._spawnCounter > this.MaximumSpawnTrap)
        {
            this.IsSpawningTrap = false;
        }

        if (Level.Instance.DistancToStartingPoint > this._distanceCounter + 20)
        {
            this._distanceCounter = Level.Instance.DistancToStartingPoint;
            this.MaximumSpawnTrap = Random.Range(1, 4);
            this.FireballSpawnRandom();
        }
    }

    private void FireballSpawnRandom()
    {
        int index = Random.Range(0, TrapSpawner.Fireballs.Length);

        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Fireballs[index],
         CameraManager.Instance.transform.position,
         Quaternion.identity
         );

        poolObject.SetParent(CameraManager.Instance.transform);
        poolObject.transform.localPosition = new Vector3(15, 0, -5);
        poolObject.gameObject.SetActive(true);

        this._spawnCounter++;
    }

}
