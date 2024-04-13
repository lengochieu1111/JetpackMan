using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CameraFollowLevelPlane _cameraFollowLevelPlane;
    public CameraFollowLevelPlane CameraFollowLevelPlane
    {
        get { return this._cameraFollowLevelPlane; }
        private set { this._cameraFollowLevelPlane = value; }
    }

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCameraFollowLevelPlane();
    }

    private void LoadCameraFollowLevelPlane()
    {
        if (this.CameraFollowLevelPlane  != null) return;

        this.CameraFollowLevelPlane = GetComponent<CameraFollowLevelPlane>();
    }
    #endregion

    public void PrepareToStartMatch()
    {
        this.CameraFollowLevelPlane?.SetIsFollowing(true);
    }

}
