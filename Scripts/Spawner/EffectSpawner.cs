using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : Spawner<EffectSpawner>
{
    [SerializeField] public readonly static string ExplosionOne = "ExplosionOne";
    [SerializeField] public readonly static string GoldCoinPickUp = "GoldCoinPickUp";
    [SerializeField] public readonly static string JetEngineSmoke = "JetEngineSmoke";

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.J))
    //    {
    //        Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.JetEngineSmoke, this.transform.position, this.transform.rotation);
    //        effect.gameObject.SetActive(true);
    //    }
    //}

}
