using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LubanResource : PlayerResource
{
    [SerializeField] int autoAttackPrimaryGen = 4;

    public override bool isResourceEnough(int primaryAmount, int secondaryAmount)
    {
        var primary = primaryResource + primaryAmount;
        var secondary = secondaryResource + secondaryAmount;

        return primary >= 0 && secondary >= 0;
    }

    public override bool changeResource(int primaryAmount, int secondaryAmount)
    {
        if (isResourceEnough(primaryAmount, secondaryAmount))
        {
            primaryResource = Mathf.Clamp(
                primaryResource + primaryAmount, 0, 100);
            secondaryResource = Mathf.Clamp(
                secondaryResource + secondaryAmount, 0, 3);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResourceAutoGen()
    {
        primaryResource = Mathf.Clamp(
            primaryResource + autoAttackPrimaryGen, 0, 100);
    }
}
