using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;

public class SummonPlayer : Singleton<SummonPlayer>
{
    private Summon cardPlaying;
    private SplatManager splat;

    public void Ready()
    {
        Refresh();
        splat.SelectSpellIndicator(cardPlaying.owner + "PointSelector");
        if (cardPlaying.ambush)
        {
            splat.CurrentSpellIndicator.SetRange(100000);
        }
        else
        {
            splat.CurrentSpellIndicator.SetRange(cardPlaying.range);
        }
        splat.CurrentSpellIndicator.Scale = (cardPlaying.radius + 1) * 2;
        splat.CancelRangeIndicator();
    }

    public void Play()
    {
        Refresh();
        cardPlaying = (Summon)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
        Instantiate(
            cardPlaying.vfx,
            splat.GetSpellCursorPosition() + cardPlaying.vfxOffset,
            Quaternion.identity);

        MinionCenter center = null;

        if (cardPlaying.centerPrefab)
        {
            center = Instantiate(
                cardPlaying.centerPrefab,
                splat.GetSpellCursorPosition(),
                Quaternion.Euler(30, 0, 0));

            center.setRadius(cardPlaying.radius);
        }

        if (cardPlaying.count == 1)
        {
            var minion = Instantiate(
                cardPlaying.minionPrefab,
                splat.GetSpellCursorPosition(),
                Quaternion.Euler(30, 0, 0));
            InitializeMinion(minion);
            if (center)
            {
                minion.transform.SetParent(center.transform);
            }
        }
        else
        {
            for (int i = 0; i < cardPlaying.count; i++)
            {
                var direction = Quaternion.Euler(0, 360/cardPlaying.count*i ,0)
                    * new Vector3(0.7f, 0, 0);
                var position = 
                    direction * cardPlaying.radius 
                    + splat.GetSpellCursorPosition();

                var minion = Instantiate(
                    cardPlaying.minionPrefab,
                    position,
                    Quaternion.Euler(30, 0, 0));
                InitializeMinion(minion);
                minion.initialPosition = position;
                if (center)
                {
                    minion.transform.SetParent(center.transform);
                }
            }
        }
    }

    private void InitializeMinion(Minion m)
    {
        m.taunt = cardPlaying.taunt;
        m.invisible = cardPlaying.invisible;
        m.guard = cardPlaying.guard;
        m.charge = cardPlaying.charge;
        m.ambush = cardPlaying.ambush;

        m.attackDamage = cardPlaying.attackDamage;
        m.attackRange = cardPlaying.attackRange;
        m.attackRate = cardPlaying.attackRate;
        m.stopRange = cardPlaying.stopRange;

        m.guardPosition = splat.GetSpellCursorPosition();
        m.guardRadius = cardPlaying.radius;
    }

    private void Refresh()
    {
        cardPlaying = (Summon)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
    }
}
