using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunItem : RyoMonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private bool _isOn = true;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _amplitude = 2f; // Độ lắc lư lên xuống
    [SerializeField] private float _frequency = 3f; // Tần số lắc lư
    private float _timeCounter;
    public bool IsOn
    {
        get { return this._isOn; }
        private set { this._isOn = value; }
    }
    public SpriteRenderer Sprite => _sprite;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSprite();
    }

    private void LoadSprite()
    {
        if (this._sprite != null) return;
        this._sprite = this.GetComponent<SpriteRenderer>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isOn = true;
        this._timeCounter = 0;
    }

    private void Update()
    {
        if (this.IsOn)
        {
            if (this.CanDestroy())
            {
                this.DestroyObject();
            }

            this.Flying();
        }
    }

    private void Flying()
    {
        this.transform.Translate(Vector2.right * this._speed * Time.deltaTime);

        this._timeCounter += Time.deltaTime;
        float newY = Mathf.Sin(this._timeCounter * this._frequency) * this._amplitude;
        this.transform.position = new Vector2(this.transform.position.x, newY);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        I_PickUpItem receiveItem = collision.GetComponent<I_PickUpItem>();
        if (receiveItem != null)
        {
            receiveItem.PickUp_GunItem();

            this.SpawnEffect();
            this.DestroyObject();
        }
    }

    private void SpawnEffect()
    {
        Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.GoldCoinPickUp, this.transform.position, this.transform.rotation);
        effect.gameObject.SetActive(true);
    }

    private bool CanDestroy()
    {
        bool canDestroy_1 = !this.Sprite.isVisible;
        bool canDestroy_2 = this.transform.position.x < CameraManager.Instance.LeftCornerOfCamera.transform.position.x;

        return canDestroy_1 & canDestroy_2;
    }

    private void DestroyObject()
    {
        ItemSpawner.Instance.Destroy(this.transform);
    }

}
