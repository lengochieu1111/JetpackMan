using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_PickUpItem
{
    public abstract void PickUp_Coin();
    public abstract void PickUp_Crystal();
    public abstract void PickUp_GunItem();
    public abstract void PickUp_Energy(int energy);
    public abstract void PickUp_DoubleCoin();
}
