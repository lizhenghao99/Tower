using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectTower
{
    public class UpgraderCardClick : Selectable
    {
        public Card card { get; private set; }

        public float cardWidth;
        public float cardHeight;

        public CanvasGroup group;

        private RectTransform rectTransform;

        private float fadeTime = 0.3f;

        private InspectMenu inspectMenu;
        private CardUpgrader upgrader;

        protected override void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        protected override void Start()
        {
            base.Start();
            rectTransform = GetComponent<RectTransform>();
            inspectMenu = FindObjectOfType<InspectMenu>();
            upgrader = FindObjectOfType<CardUpgrader>();
        }

        public void SetCard(Card c)
        {
            card = c;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            ZoomIn();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ZoomOut();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (IsInteractable())
            {
                base.OnPointerDown(eventData);
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    ClickZoom();
                    GlobalAudioManager.Instance.Play("Tap", Vector3.zero);
                    upgrader.EnterUpgrade(GetComponent<CardDisplay>(), card);
                }
                else
                {
                    inspectMenu.Enter(GetComponent<CardDisplay>(), card);
                }
            }
        }

        private void ZoomIn()
        {
            rectTransform.DOSizeDelta(
                        new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.3f)
                        .SetEase(Ease.OutQuint);
        }

        private void ZoomOut()
        {
            rectTransform.DOSizeDelta(
                    new Vector2(cardWidth, cardHeight), 0.3f)
                    .SetEase(Ease.OutQuint);
        }

        private void ClickZoom()
        {
            rectTransform.DOSizeDelta(
                        new Vector2(cardWidth * 1.25f, cardHeight * 1.25f), 0.1f)
                        .SetEase(Ease.OutQuint)
                        .OnComplete(() => rectTransform.DOSizeDelta(
                            new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.1f)
                            .SetEase(Ease.OutQuint));
        }
    }
}