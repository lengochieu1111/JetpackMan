using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : RyoMonoBehaviour
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
            AnimationOn(value);
            this._isOn = value; 
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        this.IsOn = true;
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

    private void AnimationOn(bool isOn)
    {
        this.Animator.SetBool(AnimationString.isOn, isOn);
    }

    public void DestroyObject()
    {
        this.IsOn = false;
        EffectSpawner.Instance.Destroy(this.transform);
    }

}
