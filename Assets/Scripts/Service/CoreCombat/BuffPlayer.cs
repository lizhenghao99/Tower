using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;
using System.Linq;

public class BuffPlayer : Singleton<BuffPlayer>, ICardTypePlayer
{
    private Buff cardPlaying;
    private SplatManager splat;
    private LayerMask layerMask;

    private PlayerController[] players;
    private PlayerController self;

    private List<GameObject> targets;

    private EffectManager effectManager;

    public void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        effectManager = FindObjectOfType<EffectManager>();
    }

    public void Ready()
    {
        Refresh();
        splat.CancelRangeIndicator();
        players = FindObjectsOfType<PlayerController>();
        self = players
            .Where(p => p.name == cardPlaying.owner.ToString())
            .FirstOrDefault();

        targets = new List<GameObject>();

        switch (cardPlaying.buffTarget)
        {
            case Buff.BuffTarget.Self:
                targets.Add(self.gameObject);
                break;
            case Buff.BuffTarget.OwnMinions:
                GameObject[] ownMinions = FindObjectsOfType<Minion>()
                    .Where(m => m.owner == cardPlaying.owner)
                    .Select(m => m.gameObject)
                    .ToArray();
                targets.AddRange(ownMinions);
                break;
            case Buff.BuffTarget.AllMinions:
                break;
            case Buff.BuffTarget.AllChars:
                break;
            case Buff.BuffTarget.AllFriendlies:
                break;
            case Buff.BuffTarget.AllEnemies:
                break;
            default:
                break;
        }

        SelectTargets();
    }

    public void Play()
    {
        Refresh();

        ApplyBuff();
    }

    private void Refresh()
    {
        cardPlaying = (Buff)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
    }

    private void SelectTargets()
    {
        foreach (GameObject t in targets)
        {
            if (t.GetComponent<PlayerController>())
            {
                t.GetComponent<PlayerController>().isSelected = true;
            }
            else if (t.GetComponent<Minion>())
            {
                t.GetComponent<Minion>().isSelected = true;
            }
        }
    }

    private void ApplyBuff()
    {
        foreach (GameObject t in targets)
        {
            if (t.GetComponent<PlayerController>())
            {
                t.GetComponent<PlayerController>().isSelected = false;
                t.GetComponent<PlayerHealth>().AddShieldPercent(cardPlaying.shieldPercent);
            }
            else if (t.GetComponent<Minion>())
            {
                t.GetComponent<Minion>().isSelected = false;
            }

            effectManager.Register(
                self.gameObject,
                t,
                cardPlaying.effect,
                cardPlaying.effectDuration,
                cardPlaying.effectAmount);

            if (cardPlaying.vfx)
            {
                var fx = Instantiate(cardPlaying.vfx, t.transform);
                fx.transform.position =
                    t.transform.position + cardPlaying.vfxOffset;
            }
        } 
    }
}
