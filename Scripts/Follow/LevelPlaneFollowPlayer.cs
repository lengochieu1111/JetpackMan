using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPlaneFollowPlayer : FollowTarget
{
    [SerializeField] private LevelScroller[] _levelScrollers;
    [SerializeField] private float _distanceToTarget = 5;
    [SerializeField] private bool _isReadyFollowPlayer;
    [SerializeField] private bool _isFollowingDeadPlayer;
    [SerializeField] private bool _isFollowRespawningPlayer;
    private float _currentSpeed;

    private Vector3 _newPosition;
    public LevelScroller[] LevelScrollers
    {
        get { return this._levelScrollers; }
        private set { this._levelScrollers = value; }
    }
    public Vector3 NewPosition
    {
        get { return this._newPosition; }
        private set { this._newPosition = value; }
    }
    public bool IsReadyFollowPlayer
    {
        get { return this._isReadyFollowPlayer; }
        private set { this._isReadyFollowPlayer = value; }
    }
    public bool IsFollowingDeadPlayer
    {
        get { return this._isFollowingDeadPlayer; }
        private set { this._isFollowingDeadPlayer = value; }
    }
    public bool IsFollowRespawningPlayer
    {
        get { return this._isFollowRespawningPlayer; }
        private set { this._isFollowRespawningPlayer = value; }
    }
    public float DistanceToTarget => this._distanceToTarget;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelScroller();
    }

    private void LoadPlayerCharacter()
    {
        this.Target = GameMode.Instance.Player.gameObject;
    }
    
    private void LoadLevelScroller()
    {
        if (this.LevelScrollers.Count() > 0) return;

        this.LevelScrollers = GetComponentsInChildren<LevelScroller>();
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsReadyFollowPlayer = false;
        this.IsFollowingDeadPlayer = false;
        this.IsFollowRespawningPlayer = false;
    }

    protected override void Following()
    {
        if (this.Target == null) return;

        if (this.IsFollowing)
        {
            this.FollowPlayerAlive();
        }
        else if (this.IsFollowingDeadPlayer)
        {
            this.FollowDeadPlayer();
        }
        else if (this.IsFollowRespawningPlayer)
        {
            this.FollowRespawnPlayer();
        }
    }

    private void FollowPlayerAlive()
    {
        if (this.IsReadyFollowPlayer)
        {
            this.NewPosition = new Vector3(this.Target.transform.position.x + this.DistanceToTarget, this.transform.position.y, this.transform.position.z);
            this.transform.position = this.NewPosition;
        }
        else
        {
            float distanceToPlayer = this.transform.position.x - this.Target.transform.position.x;
            if (distanceToPlayer <= this.DistanceToTarget)
            {
                this.IsReadyFollowPlayer = true;
            }
            else
            {
                float speed = Mathf.Lerp(5, 12, (distanceToPlayer - this.DistanceToTarget) * Time.deltaTime * 1.2f);
                this.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    private void FollowDeadPlayer()
    {
        float distanceToPlayer = this.transform.position.x - this.Target.transform.position.x;

        if (distanceToPlayer > 0)
        {
            float speed = Mathf.Lerp(5, 12, (distanceToPlayer - this.DistanceToTarget) * Time.deltaTime * 1.2f);
            this.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            this.NewPosition = new Vector3(this.Target.transform.position.x, this.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.NewPosition, 0.08f);
        }
    }

    private void FollowRespawnPlayer()
    {
        float distanceToPlayer = this.Target.transform.position.x + this.DistanceToTarget - this.transform.position.x;
        if (distanceToPlayer > 0)
        {
            this._currentSpeed += Time.deltaTime * 50;
            this.transform.Translate(Vector3.right * this._currentSpeed * Time.deltaTime);
        }
        else
        {
            this.IsFollowing = true;
            this.IsFollowRespawningPlayer = false;
        }
    }

    public void PrepareToStartMatch()
    {
        this.LoadPlayerCharacter();
    }

    public void StartFollowingPlayerAlive()
    {

        this.IsFollowing = true;
        this.IsFollowingDeadPlayer = false;
        this.IsFollowRespawningPlayer = false;
        this.StartAllLevelScroller();
    }

    public void StartFollowingDeadPlayer()
    {
        this.IsFollowing = false;
        this.IsFollowingDeadPlayer = true;
        this.IsFollowRespawningPlayer = false;
    }

    public void StartFollowingRespawnPlayer()
    {
        this._currentSpeed = 0;
        this.IsFollowing = false;
        this.IsFollowingDeadPlayer = false;
        this.IsFollowRespawningPlayer = true;
        this.StartAllLevelScroller();
    }

    public void StopAllLevelScroller()
    {
        this.EndAllLevelScroller();
    }

    private void StartAllLevelScroller()
    {
        foreach (LevelScroller levelScroller in this.LevelScrollers)
        {
            levelScroller?.StartScrolling();
        }
    }

    private void EndAllLevelScroller()
    {
        foreach (LevelScroller levelScroller in this.LevelScrollers)
        {
            levelScroller?.StopScrolling();
        }
    }

    public void NextLevel()
    {
        // this.Target = GameMode.Instance.Player?.gameObject;

        //this.IsFollowing = true;
        //this.IsReadyFollowPlayer = true;
    }

}
