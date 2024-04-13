using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : Spawner<TrapSpawner>
{
    [SerializeField] public static readonly string[] Bolts = { "BoltOne", "BoltTwo", "BoltThree" };
    [SerializeField] public static readonly string[] Lasers = { "BlueLaser", "GreenLaser", "RedLaser", "YellowLaser" };
    [SerializeField] public static readonly string[] Fireballs = { "BlueFireball", "PinkFireball", "RedFireball" };
    [SerializeField] public static readonly string[] LaserOne = { "LaserOne_Short", "LaserOne_Long" };

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        //Transform poolObject = TrapSpawner.Instance.Spawn(TrapSpawner.Lasers[0], CameraHolder.Instance.transform.position, this.transform.rotation);
    //        //poolObject.SetParent(CameraHolder.Instance.transform);
    //        //poolObject.transform.localPosition = new Vector3(0, 0, 5);
    //        //poolObject.gameObject.SetActive(true);
    //    }
    //}

}
