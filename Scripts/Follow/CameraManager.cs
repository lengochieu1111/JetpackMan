using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CameraFollowLevelPlane _cameraFollowLevelPlane;
    [SerializeField] private EndGame _endGame;
    [SerializeField] private Transform _rightCornerOfCamera;
    [SerializeField] private Transform _leftCornerOfCamera;
    [SerializeField] private Transform _centerOfCamera;
    [SerializeField] private Transform _widgetHolder;
    [SerializeField] private SpriteRenderer _sprite_BlackPanel;
    [SerializeField] private float _levelChangeTime = 1f;
    private Coroutine _nextLevelCoroutine;

    public EndGame EndGame => this._endGame;
    public Transform RightCornerOfCamera => this._rightCornerOfCamera;
    public Transform LeftCornerOfCamera => this._leftCornerOfCamera;
    public Transform CenterOfCamera => this._centerOfCamera;
    public Transform WidgetHolder => this._widgetHolder;
    public SpriteRenderer Sprite_BlackPanel => this._sprite_BlackPanel;
    public CameraFollowLevelPlane CameraFollowLevelPlane => this._cameraFollowLevelPlane;
    public float LevelChangeTime => this._levelChangeTime;


    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLeftCornerOfCamera();
        this.LoadRightCornerOfCamera();
        this.LoadCenterOfCamera();
        this.LoadWidgetHolder();
        this.LoadEndGame();

        this.LoadBlackPanel();
        this.LoadCameraFollowLevelPlane();
    }

    private void LoadCameraFollowLevelPlane()
    {
        if (this._cameraFollowLevelPlane != null) return;
        this._cameraFollowLevelPlane = GetComponent<CameraFollowLevelPlane>();
    }

    private void LoadLeftCornerOfCamera()
    {
        if (this._rightCornerOfCamera != null) return;
        this._rightCornerOfCamera = this.transform.Find("RightCornerOfCamera");
    }

    private void LoadRightCornerOfCamera()
    {
        if (this._leftCornerOfCamera != null) return;
        this._leftCornerOfCamera = this.transform.Find("LeftCornerOfCamera");
    }

    private void LoadCenterOfCamera()
    {
        if (this._centerOfCamera != null) return;
        this._centerOfCamera = this.transform.Find("CenterOfCamera");
    }
    
    private void LoadWidgetHolder()
    {
        if (this._widgetHolder != null) return;
        this._widgetHolder = this.transform.Find("WidgetHolder");
    }
    
    private void LoadEndGame()
    {
        if (this._endGame != null) return;
        this._endGame = this.transform.GetComponentInChildren<EndGame>(true);
    }

    private void LoadBlackPanel()
    {
        if (this._sprite_BlackPanel != null) return;
        this._sprite_BlackPanel = this.GetComponentInChildren<SpriteRenderer>(true);
    }
    #endregion

    protected override void SetupComponents()
    {
        base.SetupComponents();

        this._sprite_BlackPanel.color = new Color(0, 0, 0, 0);
        this._sprite_BlackPanel.gameObject.SetActive(false);
    }

    public void PrepareToStartMatch()
    {
        this.CameraFollowLevelPlane?.PrepareToStartMatch();
    }

    /*
     * 
     */

    public void NextLevel()
    {
        this._nextLevelCoroutine = StartCoroutine(this.NextLevel_Coroutine());
    }

    private IEnumerator NextLevel_Coroutine()
    {
        this.Sprite_BlackPanel?.gameObject.SetActive(true);

        float timeCounter_On = 0;

        while (timeCounter_On <= this.LevelChangeTime)
        {
            timeCounter_On += Time.deltaTime;

            this.Sprite_BlackPanel.color = new Color(0, 0, 0, timeCounter_On / this.LevelChangeTime);

            yield return null;
        }

    }

    public void StartEndGame()
    {
        this.EndGame?.gameObject.SetActive(true);
    }

}
