using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPlaneSpawner : Spawner<LevelPlaneSpawner>
{
    [SerializeField] public static readonly string[] LevelPlanes = 
        { "Level_Plane_1", "Level_Plane_2", "Level_Plane_3", "Level_Plane_4", "Level_Plane_5" };
    [SerializeField] public static int CurrentIndex = 0;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Transform poolObject = LevelPlaneSpawner.Instance.Spawn(LevelPlaneSpawner.LevelPlanes[1], transform.position, Quaternion.identity);
    //        poolObject.SetParent(Level.Instance.transform);
    //        poolObject.localPosition = Vector3.zero;
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}

    public static void SpawnLevel(int levelIndex)
    {
        Vector3 spawnPosition = Level.Instance.transform.position;
        Quaternion spawnRotation = Level.Instance.transform.rotation;

        Transform poolObject = LevelPlaneSpawner.Instance.Spawn(LevelPlaneSpawner.LevelPlanes[levelIndex], spawnPosition, spawnRotation);
        poolObject.SetParent(Level.Instance.transform);
        poolObject.localPosition = Vector3.zero;
        poolObject.gameObject.SetActive(true);
        CurrentIndex++;
    }

}
