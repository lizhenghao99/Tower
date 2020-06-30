using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using System.Linq;
using System.Diagnostics;

public class BuffPlayer : Singleton<BuffPlayer>
{
    private Buff cardPlaying;
    private SplatManager splat;
    private LayerMask layerMask;

    private PlayerController[] players;
    private PlayerController self;

    public void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy");
    }

    public void Ready()
    {
        Refresh();
        splat.CancelRangeIndicator();
        players = FindObjectsOfType<PlayerController>();
        self = players
            .Where(p => p.name == cardPlaying.owner.ToString())
            .FirstOrDefault();
        switch (cardPlaying.buffTarget)
        {
            case Buff.BuffTarget.AllPlayers:
                break;
            case Buff.BuffTarget.AllScreen:
                break;
            case Buff.BuffTarget.Self:
                self.isSelected = true;
                break;
            default:
                break;
        }
    }

    public void Play()
    {
        Refresh();
        self.transform.parent.GetComponentInChildren<SpriteRenderer>().flipX = false;
        switch (cardPlaying.buffTarget)
        {
            case Buff.BuffTarget.AllPlayers:
                break;
            case Buff.BuffTarget.AllScreen:
                break;
            case Buff.BuffTarget.Self:
                self.isSelected = false;
                self.GetComponent<PlayerHealth>().AddShield(cardPlaying.shieldAmount);
                break;
            default:
                break;
        }
    }

    private void Refresh()
    {
        cardPlaying = (Buff)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
    }
}
