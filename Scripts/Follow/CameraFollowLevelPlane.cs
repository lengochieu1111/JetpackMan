using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLevelPlane : FollowTarget
{
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelPlane();
    }

    private void LoadLevelPlane()
    {
        if (this.Target != null) return;
        this.Target = Level.Instance?.LevelPlane?.gameObject;
    }

    private void LateUpdate()
    {
        this.Following();
    }

}
