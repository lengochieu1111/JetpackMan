using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShellCasing : RyoMonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _sprite;
    public SpriteRenderer Sprite => _sprite;
    public Rigidbody2D Rigidbody => _rigidbody;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSprite();
        this.LoadRigidbody();
    }

    private void LoadRigidbody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = this.GetComponent<Rigidbody2D>();
    }
    
    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        float xForce = Random.Range(600f, -100f);
        float yForce = Random.Range(-300f, -600f);

        this.Rigidbody.AddForce(new Vector2(xForce, yForce));
    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.DestroyObject();
        }

    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;
        return canDestroy_1 & canDestroy_2;
    }

    public void DestroyObject()
    {
        LevelElementSpawner.Instance.Destroy(this.transform);
    }

}
