using MVCS.Architecture.BaseCharacter;
using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePortal : Singleton<SpacePortal>
{
    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _hasChangedLevel;
    public CapsuleCollider2D CapsuleCollider => this._capsuleCollider;
    public SpriteRenderer Sprite => _sprite;
    public bool HasChangedLevel
    {
        get { return this._hasChangedLevel; }
        private set { this._hasChangedLevel = value; }
    }
    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCapsuleCollider();
        this.LoadSprite();
    }

    private void LoadCapsuleCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
    }

    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.HasChangedLevel = false;

    }

    private void Update()
    {
        if (this.CanDestroy())
        {
            this.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseCharacter baseCharacter = collision.GetComponent<BaseCharacter>();
        if (baseCharacter != null && this.HasChangedLevel == false)
        {
            this.HasChangedLevel = true;
            this.gameObject.SetActive(false);
            this.NextLevel();
        }

    }

    private void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }

}
