using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : RyoMonoBehaviour
{
    [Header("Property")]
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _x = 0.2f, _y = 0.2f;
    public RawImage RawImage => _rawImage;

    #region Load Component
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadRawImage();
    }

    private void LoadRawImage()
    {
        if (this._rawImage != null) return;

        this._rawImage = GetComponent<RawImage>();
    }
    #endregion


    private void Update()
    {
        this.RawImage.uvRect = 
            new Rect(this.RawImage.uvRect.position + new Vector2(this._x, this._y) * Time.deltaTime, this.RawImage.uvRect.size);
    }

}
