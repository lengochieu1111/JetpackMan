using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner<EnemySpawner>
{
    [SerializeField] public static readonly string Dr_Bones = "Dr_Bones";

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Transform poolObject = EnemySpawner.Instance.Spawn(EnemySpawner.Dr_Bones, transform.position, transform.rotation);
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}
}
