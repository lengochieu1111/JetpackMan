using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : Spawner<EffectSpawner>
{
    [SerializeField] public readonly static string Explosion_1 = "Explosion_1";

/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.Explosion_1, this.transform.position, this.transform.rotation);
            effect.gameObject.SetActive(true);
        }
    }*/

}
