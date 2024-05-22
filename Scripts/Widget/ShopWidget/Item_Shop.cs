using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_Shop : RyoMonoBehaviour
{
    [SerializeField] Transform _opened;
    [SerializeField] Transform _unopened;
    [SerializeField] Transform _inUse;
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadOpened();
        this.LoadUnopened();
        this.LoadInUse();
    }

    private void LoadOpened()
    {
        if (this._opened != null) return;
        this._opened = this.FindChildByName(this.transform, "Opened");
        this._opened.gameObject.SetActive(false);
    }
    
    private void LoadUnopened()
    {
        if (this._unopened != null) return;
        this._unopened = this.FindChildByName(this.transform, "Unopened");
        this._unopened.gameObject.SetActive(false);
    }

    private void LoadInUse()
    {
        if (this._inUse != null) return;
        this._inUse = this.FindChildByName(this.transform, "InUse");
        this._inUse.gameObject.SetActive(false);
    }

    private Transform FindChildByName(Transform parrentObject, string childName)
    {
        Transform childObject = parrentObject.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrentObject)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }

    }

    /*
     * 
     */

    public void Opened()
    {
        this._opened?.gameObject.SetActive(true);
        this._unopened?.gameObject.SetActive(false);
        this._inUse?.gameObject.SetActive(false);

    }
    
    public void Unopened()
    {
        this._opened?.gameObject.SetActive(false);
        this._unopened?.gameObject.SetActive(true);
        this._inUse?.gameObject.SetActive(false);
    }
    
    public void InUse()
    {
        this._opened?.gameObject.SetActive(false);
        this._unopened?.gameObject.SetActive(false);
        this._inUse?.gameObject.SetActive(true);
    }


}
