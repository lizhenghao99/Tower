using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DaoshiResource : PlayerResource
{
    [SerializeField] int autoPrimaryRegen = 4;

    public override bool IsResourceEnough(int primaryAmount, int secondaryAmount)
    {
        var primary = primaryResource + primaryAmount;

        return primary >= 0;
    }

    public override bool ChangeResource(int primaryAmount, int secondaryAmount)
    {
        if (IsResourceEnough(primaryAmount, secondaryAmount))
        {
            primaryResource = Mathf.Clamp(
                primaryResource + primaryAmount, 0, 100);
            if (secondaryAmount != -1)
            {
                secondaryResource = secondaryAmount;
            }
            OnResourceChanged();
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator ResourceAutoGen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (secondaryResource == 0)
            {
                ChangeResource(autoPrimaryRegen * 2, -1);
            }
            else
            {
                ChangeResource(autoPrimaryRegen, -1);
            }
        }
    }

    private void OnResourceChanged()
    {
        resourceChanged?.Invoke(gameObject, EventArgs.Empty);
    }
}
