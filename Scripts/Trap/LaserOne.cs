using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserOne : RyoMonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float[] zAxisSpawn_One = { 20f, 50f };
    [SerializeField] private float[] zAxisSpawn_Two = { 130f, 170f };

    [SerializeField] private float[] yAxisSpawn_Min_One = { -4f, -2f };
    [SerializeField] private float[] yAxisSpawn_Max_One = { 1.5f, 3.5f };

    [SerializeField] private float[] yAxisSpawn_Min_Two = { -5f, -2f };
    [SerializeField] private float[] yAxisSpawn_Max_Two = { 1.5f, 4.5f };

    public float Damage => _damage;
    public CapsuleCollider2D CapsuleCollider
    {
        get { return this._capsuleCollider; }
        private set { this._capsuleCollider = value; }
    }
    public SpriteRenderer[] Sprites
    {
        get { return this._sprites; }
        private set { this._sprites = value; }
    }

    #region Component & Value
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSprites();
        this.LoadCapsuleCollider();
    }

    private void LoadCapsuleCollider()
    {
        if (this.CapsuleCollider != null) return;
        this.CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void LoadSprites()
    {
        if (this._sprites.Count() > 0) return;
        this._sprites = this.GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this.CapsuleCollider.isTrigger = true;
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

        this.RandomRotation();
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            TrapSpawner.Instance.Destroy(this.transform);
        }
    }

    private void RandomRotation()
    {
        float zAxisSpawnRandom;
        do
        {
            zAxisSpawnRandom = Random.Range(this.zAxisSpawn_One[0], this.zAxisSpawn_Two[1]);
        }
        while (zAxisSpawnRandom > this.zAxisSpawn_One[1] && zAxisSpawnRandom < this.zAxisSpawn_Two[0]);

        this.transform.rotation = Quaternion.Euler(0, 0, zAxisSpawnRandom);
        this.RandomPositionAccordinToRotation(zAxisSpawnRandom);
    }

    private void RandomPositionAccordinToRotation(float zAxisSpawnRandom)
    {
        float[] zAxisSpawn = this.zAxisSpawn_One;
        float[] yAxis_Min = this.yAxisSpawn_Min_One;
        float[] yAxis_Max = this.yAxisSpawn_Max_One;

        if (zAxisSpawnRandom >= this.zAxisSpawn_Two[0])
        {
            zAxisSpawn = this.zAxisSpawn_Two;
            yAxis_Min = this.yAxisSpawn_Min_Two;
            yAxis_Max = this.yAxisSpawn_Max_Two;
        }

        float lenghtRatio = (zAxisSpawn[1] - zAxisSpawn[0]) / (yAxis_Min[1] - yAxis_Min[0]);
        float ratio = (zAxisSpawnRandom - zAxisSpawn[0]) / lenghtRatio;
        float ySpawnMin = yAxis_Min[0] + ratio;
        float ySpawnMax = yAxis_Max[0] + ratio;

        float yAxisSpawnRandom = Random.Range(ySpawnMin, ySpawnMax);
        this.transform.position = new Vector3(this.transform.position.x, yAxisSpawnRandom, this.transform.position.z);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = true;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        foreach(SpriteRenderer sprite in this._sprites)
        {
            if (sprite.isVisible)
            {
                canDestroy_1 = false;
                break;
            }
        }

        return canDestroy_1 & canDestroy_2;
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
