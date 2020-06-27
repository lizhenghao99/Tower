using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LubanResource : PlayerResource
{
    public override bool changeResource(int primaryAmount, int secondaryAmount)
    {
        var primary = primaryResource + primaryAmount;
        var secondary = secondaryResource + secondaryAmount;

        if (primary < 0 || secondary < 0)
        {
            return false;
        }
        else
        {
            primaryResource = primary;
            secondaryResource = secondary;
            return true;
        }
    }
}
