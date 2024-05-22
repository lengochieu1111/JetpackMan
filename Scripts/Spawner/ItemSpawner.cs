using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner<ItemSpawner>
{
    [Header("Old")]
    [SerializeField] public static readonly string Coin = "Coin";
    [SerializeField] public static readonly string DynamicCoin = "DynamicCoin";
    [SerializeField] public static readonly string BigCoin = "BigCoin";
    [SerializeField] public static readonly string GunItem = "GunItem";
    [SerializeField] public static readonly string Crystal = "Crystal";
    [SerializeField] public static readonly string Magnet = "Magnet";


    [Header("New")]
    [SerializeField] public static readonly string[] CoinMeshs = 
        { "CoinMesh_HIEU", "CoinMesh_HEART", "CoinMesh_LONG", "CoinMesh_CNPM", "CoinMesh_WAVE" };
    
    [SerializeField] public static readonly string[] AdvancedItems = 
        { "Crystal", "BigCoin", "Magnet", "DoubleCoin"};

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Vector3 spawnPos = CameraManager.Instance.CenterOfCamera.position;
    //        spawnPos.x += 5;
    //        Transform poolObject = ItemSpawner.Instance.Spawn(ItemSpawner.DynamicCoin, spawnPos, transform.rotation);
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}

}
