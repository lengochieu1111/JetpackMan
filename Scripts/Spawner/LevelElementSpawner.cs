using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElementSpawner : Spawner<LevelElementSpawner>
{
    [SerializeField] public static readonly string BulletShellCasing = "BulletShellCasing";

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.H))
    //    {
    //        Transform poolObject = LevelElementSpawner.Instance.Spawn(LevelElementSpawner.BulletShellCasing, transform.position, Quaternion.identity);
    //        //poolObject.SetParent(GameMode.Instance.Player.View.transform);
    //        //poolObject.localPosition = new Vector2(16.3f, 0.1f);
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}

}

