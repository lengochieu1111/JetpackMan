using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FollowTarget : RyoMonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected bool isFollowing;
    public GameObject Target
    {
        get { return target; }
        protected set { target = value; }
    }
    public bool IsFollowing
    {
        get { return isFollowing; }
        protected set { isFollowing = value; }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsFollowing = false;
    }

    protected virtual void FixedUpdate()
    {
        Following();
    }

    protected virtual void Following()
    {
        if (this.Target && this.IsFollowing)
            this.transform.position = this.target.transform.position;
    }

    public void SetIsFollowing(bool isFollowing)
    {
        this.IsFollowing = isFollowing;
    }

}
