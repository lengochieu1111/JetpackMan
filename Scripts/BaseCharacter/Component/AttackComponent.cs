using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackComponent : BaseCharacterAbstract
{
    [SerializeField] private bool _isCombatMode;
    [SerializeField] private float _bulletCooldownTime = 0.2f;
    private float _timeCounter;
    public float BulletCooldownTime => _bulletCooldownTime;
    public bool IsCombatMode
    {
        get { return _isCombatMode; }
        private set { _isCombatMode = value; }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isCombatMode = false;
    }

    private void Update()
    {
        if (this.IsCombatMode)
        {
            if (Character.CurrentBullet == 0)
            {
                if (this._timeCounter >= 0.8f)
                {
                    this.ShootBullet();

                    this._timeCounter = 0;
                }
                else
                    this._timeCounter += Time.deltaTime;
            }
            else
            {
                if (this._timeCounter >= this.BulletCooldownTime)
                {
                    this.ShootBullet();

                    this._timeCounter = 0;
                }
                else
                    this._timeCounter += Time.deltaTime;
            }
        }
    }

    private void ShootBullet()
    {
        int index = Character.CurrentBullet;
        Transform poolObject = BulletSpawner.Instance.Spawn(BulletSpawner.PlayerGunBullets[index], transform.position, Quaternion.identity);
        poolObject.SetParent(this.transform);

        if (index == 0)
        {
            poolObject.localPosition = new Vector2(13f, 0.1f);
        }
        else
        {
            poolObject.localPosition = new Vector2(1f, 0.1f);
        }

        poolObject.gameObject.SetActive(true);
    }

    public void RequestCombatMode(bool isCombatMode)
    {
        this.IsCombatMode = isCombatMode;
    }




}
