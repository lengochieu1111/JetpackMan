using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLevelPlane : FollowTarget
{
    private void LoadTarget()
    {
        this.Target = Level.Instance?.LevelPlane?.gameObject;
    }

    private void LateUpdate()
    {
        this.Following();
    }

    public void PrepareToStartMatch()
    {
        this.LoadTarget();

        this.IsFollowing = true;
    }

}
