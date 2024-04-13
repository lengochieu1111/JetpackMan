using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelScroller : RyoMonoBehaviour
{
    [SerializeField] private Material _material;

    [Range(-1.0f, 1.0f)]
    [SerializeField] private float _scrollerSpeed = 0.5f;
    [SerializeField] private bool _isScrolling;
    [SerializeField] private int _scrollDirectin = 1;
    public Material Material
    {
        get { return this._material; }
        private set { this._material = value; }
    }
    public bool IsScrolling
    {
        get { return this._isScrolling; }
        private set { this._isScrolling = value; }
    }
    private float _offset;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadMaterial();
    }

    private void LoadMaterial()
    {
        if (this.Material != null) return;

        this.Material = GetComponent<Renderer>().material;
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsScrolling = false;
    }

    private void FixedUpdate()
    {
        if (this.Material && this.IsScrolling)
        {
            this.UpdateTextureOffset();
        }
    }

    private void UpdateTextureOffset()
    {
        this._offset += (Time.fixedDeltaTime * this._scrollerSpeed * this._scrollDirectin) / 4;
        this.Material.SetTextureOffset("_MainTex", new Vector2(this._offset, 0));
    }

    public void StopScrolling()
    {
        this.IsScrolling = false;
    }

    public void StartScrolling()
    {
        this.IsScrolling = true;
    }

}
