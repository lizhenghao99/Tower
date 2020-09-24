using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ProjectTower;
using DG.Tweening;

namespace ProjectTower
{
    public class LubanDiscardButton : DiscardButton
    {
        protected override void PointerEnterBehavior()
        {
            gameObject.transform.DOLocalRotate(
                new Vector3(0, 0, 20), 0.3f)
                .SetEase(Ease.OutQuint);
        }

        protected override void PointerExitBehavior()
        {
            gameObject.transform.DOLocalRotate(
                   new Vector3(0, 0, 0), 0.3f)
                   .SetEase(Ease.OutQuint);
        }

        protected override void PointerDownBehavior()
        {
            gameObject.transform.DOPunchRotation(
                    new Vector3(0, 0, 45), 0.5f)
                    .SetEase(Ease.OutQuint);

            GlobalAudioManager.Instance.Play("Hammer", Vector3.zero);
        }

        protected override void DeselecteBehavior()
        {
            gameObject.transform.DOLocalRotate(
                    new Vector3(0, 0, 0), 0.3f)
                    .SetEase(Ease.OutQuint);
        }

        protected override bool DiscardBehavior(Card card)
        {
            player.setIsCasting(true);
            StartCoroutine(Utils.Timeout(() =>
                player.setIsCasting(false), 10f));

            return true;
        }
    }
}