using DG.DemiLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;

public class MinionRangeMouseOver : MonoBehaviour
{
    [SerializeField] SplatManager splat;
    private SplatManager mySplat;
    
    public void setRadius(float radius)
    {
        mySplat = Instantiate(splat);
        mySplat.transform.SetParent(gameObject.transform);
        mySplat.transform.localPosition = new Vector3(0, 0, 0);

        var collider = GetComponent<CapsuleCollider>();
        collider.radius = radius;
        collider.height = 0;
        collider.center = new Vector3(0, 0, 0);

        mySplat.GetRangeIndicator("Range").DefaultScale = (radius + 1) * 2;
    }

    private void OnMouseOver()
    {
        mySplat.SelectRangeIndicator("Range");
    }

    private void OnMouseExit()
    {
        mySplat.CancelRangeIndicator();
    }
}
