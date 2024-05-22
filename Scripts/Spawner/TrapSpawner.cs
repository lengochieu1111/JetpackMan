using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : Spawner<TrapSpawner>
{
    [SerializeField] public static readonly string[] Bolts = { "BoltOne", "BoltTwo", "BoltThree" };
    [SerializeField] public static readonly string[] Lasers = { "BlueLaser", "GreenLaser", "RedLaser", "YellowLaser" };
    [SerializeField] public static readonly string[] Fireballs = { "BlueFireball", "PinkFireball", "RedFireball" };
    
    [SerializeField] public static readonly string[] LaserOne = { "LaserOne_Short", "LaserOne_Long" };
    [SerializeField] public static readonly string LaserTwo = "LaserTwo";
    [SerializeField] public static readonly string Rocket = "Rocket";


    [SerializeField] public static readonly string LaserGunBullet = "LaserGunBullet";
    [SerializeField] public static readonly string[] LaserGun_One = { "LaserGun_One_Top", "LaserGun_One_Down" };

    [Header("Boss")]
    [SerializeField] public static readonly string Missile_Default = "Missile_Default";
    [SerializeField] public static readonly string Missile_Guided = "Missile_Guided";
    [SerializeField] public static readonly string UFO_One = "UFO_One";

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Vector3 spawnPos = CameraManager.Instance.RightCornerOfCamera.transform.position;
    //        spawnPos.x += 5;
    //        Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Missile_Guided, spawnPos, this.transform.rotation);
    //        // poolObject.SetParent(CameraManager.Instance.LeftCornerOfCamera.transform);
    //        // poolObject.transform.localPosition = new Vector3(0, 0, 0);
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}

}
