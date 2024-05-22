using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _isOn = false;
    public Animator Animator
    {
        get { return this._animator; }
        set { this._animator = value; }
    }

    public bool IsOn
    {
        get { return this._isOn; }
        set 
        {
            if (value)
                this.AnimationOn();

            this._isOn = value; 
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadAnimator();
    }

    private void LoadAnimator()
    {
        if (this.Animator != null) return;
        this.Animator = GetComponent<Animator>();
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isOn = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.IsOn = true;
    }

    private void AnimationOn()
    {
        this.Animator.SetTrigger(AnimationString.isOn);
    }

    public void DestroyObject()
    {
        EffectSpawner.Instance.Destroy(this.transform);
    }

}
