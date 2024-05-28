using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnTraps : RyoMonoBehaviour
{
    [Header("Trap")]
    [SerializeField] private bool _canSpawnTrap;
    [SerializeField] private bool _isSpawningTrap;
    [SerializeField] private float[] _nextTrapSpawnDistance = { 80f, 120f };
    [SerializeField] private int _maximumSpawnTrap = 1;
    [SerializeField] private int _trapType;
    private float _trapSpawnDistance;
    private int _spawnCounter;
    private float _distanceCounter;
    private float _randomReproductionDistance;
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
    public float[] NextTrapSpawnDistance => this._nextTrapSpawnDistance; 
    public int MaximumSpawn
    {
        get { return this._maximumSpawnTrap; }
        private set { this._maximumSpawnTrap = value; }
    }
    public int TrapType
    {
        get { return this._trapType; }
        private set { this._trapType = value; }
    }
    public bool CanSpawnTrap
    {
        get { return this._canSpawnTrap; }
        private set { this._canSpawnTrap = value; }
    }

    #region Property Laser One
    [Header("Laser One")]
    [SerializeField] private bool _isSpawningLaserOne;
    [SerializeField] private float[] _laserOneReproductionDistance = { 20, 40f };
    [SerializeField] private int[] _laserOneMaximumReproduction = { 1, 3 };
    public bool IsSpawningLaserOne
    {
        get { return this._isSpawningLaserOne; }
        private set 
        { 
            if (value && this.IsSpawningLaserOne == false)
            {
                this._randomReproductionDistance = Random.Range(this.LaserOneReproductionDistance[0], this.LaserOneReproductionDistance[1]);
                this.MaximumSpawn = Random.Range(this.LaserOneMaximumReproduction[0], this.LaserOneMaximumReproduction[1] + 1);
            }

            this._isSpawningLaserOne = value;
        }
    }
    public float[] LaserOneReproductionDistance => _laserOneReproductionDistance;
    public int[] LaserOneMaximumReproduction => _laserOneMaximumReproduction;
    #endregion

    #region Property Light Bullet
    [Header("Light Bullet")]
    [SerializeField] private bool _isSpawningLightBullet;
    [SerializeField] private float[] _lightBulletReproductionDistance = { 10f, 20f };
    [SerializeField] private int[] _lightBulletMaximumReproduction = { 1, 3 };
    public bool IsSpawningLightBullet
    {
        get { return this._isSpawningLightBullet; }
        private set 
        {
            if (value && this.IsSpawningLightBullet == false)
            {
                this._randomReproductionDistance = Random.Range(this.LightBulletReproductionDistance[0], this.LightBulletReproductionDistance[1]);
                this.MaximumSpawn = Random.Range(this.LightBulletMaximumReproduction[0], this.LightBulletMaximumReproduction[1] + 1);
            }

            this._isSpawningLightBullet = value;
        }
    }
    public float[] LightBulletReproductionDistance => _lightBulletReproductionDistance;
    public int[] LightBulletMaximumReproduction => _lightBulletMaximumReproduction;
    #endregion

    #region Property Laser Two
    [Header("Laser Two")]
    [SerializeField] private bool _isSpawningLaserTwo;
    [SerializeField] private float[] _laserSpawn_xAxis = { 6f, 3.5f, 1f, -1.5f, -4f };
    private List<int> _spawnedIndexs = new List<int>();
    public bool IsSpawningLaserTwo
    {
        get { return this._isSpawningLaserTwo; }
        private set 
        { 
            if (value && this.IsSpawningLaserTwo == false)
            {
                this._spawnedIndexs.Clear();
                this.MaximumSpawn = Random.Range(1, this.LaserSpawn_xAxis.Length - 1);
            }
            this._isSpawningLaserTwo = value; 
        }
    }
    public float[] LaserSpawn_xAxis => _laserSpawn_xAxis;
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsSpawningTrap = false; 
        this.CanSpawnTrap = true; 
        this.IsSpawningLaserTwo = false;
        this.IsSpawningLaserOne = false;
        this.IsSpawningLightBullet = false;
        this.TrapSpawnDistance = Random.Range(this.NextTrapSpawnDistance[0], this.NextTrapSpawnDistance[1]);
    }

    private void Update()
    {
        if (this.CanSpawnTrap == false) return;

        this.RandomSpawnTrap();

        if (ItemSpawner.Instance.SpawnedCount > 0) return;

        if (this.IsSpawningLaserOne)
        {
            this.LaserOneSpawnProcess();
        }
        else if (this.IsSpawningLightBullet)
        {
            this.LightBulletSpawnProcess();
        }
        else if (this.IsSpawningLaserTwo)
        {
            this.LaserTwoSpawnProcess();
        }

    }

    private void RandomSpawnTrap()
    {
        if (Level.Instance.DistancToStartingPoint > this.TrapSpawnDistance && this.IsSpawningTrap == false)
        {
            this.IsSpawningTrap = true;
        }
    }

    private void StartTrapSpawning()
    {
        this._distanceCounter = Level.Instance.DistancToStartingPoint;
        this._spawnCounter = 0;
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
                this.IsSpawningLaserTwo = true;
                break;
            case 1:
                this.IsSpawningLaserOne = true;
                break;
            case 2:
                this.IsSpawningLightBullet = true;
                break;
        }
    }

    private void EndTrapSpawning()
    {
        this.TrapSpawnDistance = Random.Range(this.NextTrapSpawnDistance[0], this.NextTrapSpawnDistance[1]);
        this.TrapSpawnDistance += Level.Instance.DistancToStartingPoint;
        this._spawnCounter = 0;

        this.IsSpawningLaserOne = false;
        this.IsSpawningLaserTwo = false;
        this.IsSpawningLightBullet = false;
    }

    #region Laeser One
    /*
     * Bolt
     */
    private void LaserOneSpawnProcess()
    {
        if (this._spawnCounter >= this.MaximumSpawn)
        {
            this.IsSpawningTrap = false;
        }
        else
        {
            if (Level.Instance.DistancToStartingPoint > this._distanceCounter + this._randomReproductionDistance)
            {
                this._randomReproductionDistance = Random.Range(this.LaserOneReproductionDistance[0], this.LaserOneReproductionDistance[1]);
                this._distanceCounter = Level.Instance.DistancToStartingPoint;
                this.LaserOneSpawnRandom();
            }
        }
    }

    private void LaserOneSpawnRandom()
    {
        int index = Random.Range(0, TrapSpawner.LaserOne.Length);

        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.LaserOne[index],
             new Vector3(CameraManager.Instance.RightCornerOfCamera.position.x + 10, 0, 0),
             this.transform.rotation);

        poolObject.gameObject.SetActive(true);

        this._spawnCounter++;
    }

    #endregion

    #region Laser Two
    /*
     * Laser
     */
    private void LaserTwoSpawnProcess()
    {
        this.LaserTwoSpawnRandom();
        this.IsSpawningTrap = false;
    }

    private void LaserTwoSpawnRandom()
    {
        for (int i = 0; i < this.MaximumSpawn; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, this.LaserSpawn_xAxis.Length);
            }
            while (this._spawnedIndexs.Contains(randomIndex));

            this._spawnedIndexs.Add(randomIndex);

            Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.LaserTwo,
                 CameraManager.Instance.CenterOfCamera.position,
                 this.transform.rotation);

            poolObject.SetParent(CameraManager.Instance.CenterOfCamera);
            poolObject.transform.localPosition = new Vector3(0, this.LaserSpawn_xAxis[randomIndex], 0);
            poolObject.gameObject.SetActive(true);
        }

    }

    #endregion

    #region Light Bullet
    /*
     * LightBullet
     */

    private void LightBulletSpawnProcess()
    {
        if (this._spawnCounter >= this.MaximumSpawn)
        {
            this.IsSpawningTrap = false;
        }

        if (Level.Instance.DistancToStartingPoint > this._distanceCounter + this._randomReproductionDistance)
        {
            this._randomReproductionDistance = Random.Range(this.LightBulletReproductionDistance[0], this.LightBulletReproductionDistance[1]);
            this._distanceCounter = Level.Instance.DistancToStartingPoint;
            this.LightBulletSpawnRandom();
        }
    }

    private void LightBulletSpawnRandom()
    {
        // int index = Random.Range(0, TrapSpawner.Fireballs.Length);
        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Rocket,
         CameraManager.Instance.RightCornerOfCamera.position,
         this.transform.rotation);

        poolObject.SetParent(CameraManager.Instance.RightCornerOfCamera);
        poolObject.transform.localPosition = new Vector3(2, 0, 0);
        poolObject.gameObject.SetActive(true);

        this._spawnCounter++;
    }
    #endregion

    /*
     * 
     */
    public void SetCanSpawnTrap(bool canSpawnTrap)
    {
        this.CanSpawnTrap = canSpawnTrap;
    }

    public void FinalStageOfTheMap()
    {
        // 
    }

    public void DestroyTrap()
    {
        foreach(Transform trap in TrapSpawner.Instance.Holder)
        {
            trap.gameObject.SetActive(false);
        }
    }

}
