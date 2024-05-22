using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner<BulletSpawner>
{
    [SerializeField] public static readonly string PlayerGunBullet_Laser = "PlayerGunBullet_Laser";
    [SerializeField] public static readonly string PlayerGunBullet_Broad = "PlayerGunBullet_Broad";
    [SerializeField] public static readonly string PlayerGunBullet_Curve = "PlayerGunBullet_Curve";
    [SerializeField] public static readonly string LightBullet_Purple = "LightBullet_Purple";
    [SerializeField]
    private Vector2[] _skillSpawnPosition = new Vector2[]
    { new Vector2( 4.9f, 6.5f ), new Vector2( 7.78f, 3.27f ) , new Vector2( 3.44f, -0.3f ) ,
            new Vector2( 2.42f, -6.15f ) , new Vector2( 5.33f, -3.79f ) , new Vector2( 7.73f, -0.57f ) };

    [SerializeField] public static readonly string[] PlayerGunBullets =
        { "PlayerGunBullet_Laser", "PlayerGunBullet_Broad", "PlayerGunBullet_Curve" };

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        //Vector3 spawnPosition = CameraManager.Instance.LeftCornerOfCamera.position;
    //        //spawnPosition.x += 2;
    //        //Transform poolObject = BulletSpawner.Instance.Spawn(BulletSpawner.LightBullet_Purple, spawnPosition, Quaternion.identity);
    //        ////poolObject.SetParent(GameMode.Instance.Player.View.transform);
    //        ////poolObject.localPosition = new Vector2(16.3f, 0.1f);
    //        //poolObject.gameObject.SetActive(true);
    //    }
    //}

}

