using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner<ItemSpawner>
{
    [SerializeField] public static readonly string GoldCoin = "GoldCoin";
    [SerializeField] public static readonly string SilverCoin = "SilverCoin";

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        Transform poolObject = ItemSpawner.Instance.Spawn(ItemSpawner.GoldCoin, transform.position, transform.rotation);
    //        poolObject.gameObject.SetActive(true);
    //    }
    //}

}
