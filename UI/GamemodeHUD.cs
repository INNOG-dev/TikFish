using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeHUD
{
    protected GameObject hudObject;

    public GamemodeHUD(GameObject HUDObject)
    {
        this.hudObject = HUDObject;

        InitHUD();
    }

    public abstract void InitHUD();

    public abstract void Update(PartyManager manager);

    public void setActive(bool active)
    {
        hudObject.SetActive(active);
    }

    public bool isActive()
    {
        return hudObject.activeInHierarchy;
    }





}
