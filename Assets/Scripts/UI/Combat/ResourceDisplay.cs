using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class ResourceDisplay : MonoBehaviour
{
    [SerializeField] protected float primaryStartPercent = 0.16f;
    [SerializeField] protected float PrimaryEndPercent = 0.76f;

    [HideInInspector] public PlayerController player;

    protected Drawer drawer;
    protected PlayerResource playerResource;
    protected Image primaryResource;
    protected Image primaryGlow;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        drawer = GetComponentInParent<Drawer>();
        playerResource = player.GetComponent<PlayerResource>();

        primaryResource = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "PrimaryDisplayFill")
                            .FirstOrDefault();
        primaryGlow = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "PrimaryDisplayGlow")
                            .FirstOrDefault();

        playerResource.resourceChanged += OnResourceChanged;
    }

    protected abstract void OnResourceChanged(object sender, EventArgs e);
}
