using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ProjectTower
{
    public class PauseMenuAnimation : MonoBehaviour
    {
        [SerializeField] Image background;
        [SerializeField] Image shadow;
        [SerializeField] RectTransform bottom;
        [SerializeField] CanvasGroup buttons;

        private float animationTime = 0.3f;
        private Vector3 position;

        private void Awake()
        {
            position = bottom.localPosition;
            bottom.localPosition += new Vector3(0, 2000, 0);
        }

        private void OnEnable()
        {
            StartCoroutine(ShowButtons());
            background.DOFillAmount(1, animationTime)
                .SetEase(Ease.OutQuint).SetUpdate(true);
            shadow.DOFillAmount(1, animationTime)
                .SetEase(Ease.OutQuint).SetUpdate(true);
            bottom.transform.DOLocalMove(position, animationTime)
                .SetEase(Ease.OutQuint).SetUpdate(true);
        }

        private void OnDisable()
        {
            background.fillAmount = 0f;
            shadow.fillAmount = 0f;
            bottom.localPosition += new Vector3(0, 2000, 0);
            buttons.alpha = 0f;
        }

        private IEnumerator ShowButtons()
        {
            yield return new WaitForSecondsRealtime(animationTime - 0.1f);
            buttons.DOFade(1, animationTime)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true);
        }
    }
}