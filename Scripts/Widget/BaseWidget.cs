using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWidget : RyoMonoBehaviour
{
    protected override void OnEnable()
    {
        base.OnEnable();

        this.transform.SetParent(CameraManager.Instance.WidgetHolder);
        this.transform.localPosition = Vector3.zero;
    }

    //protected override void OnDisable()
    //{
    //    base.OnDisable();

    //    if (this.gameObject.activeSelf == false) return;

    //    if (GameMode.Instance?.HUD != null)
    //    {
    //        this.transform.SetParent(GameMode.Instance.HUD.transform);
    //        this.transform.localPosition = Vector3.zero;
    //    }
    //}

}
