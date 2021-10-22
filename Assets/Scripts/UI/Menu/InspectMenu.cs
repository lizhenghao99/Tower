using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

namespace ProjectTower
{
    public class InspectMenu : MonoBehaviour
    {
        [SerializeField] GameObject inspectMenu;
        [SerializeField] Vector2 cardPos = new Vector2(-1000, 250);
        [SerializeField] Vector2 cardSize = new Vector2(230, 300) * 3.6f;

        public static bool isInspecting = false;
        public static bool isPaused = false;

        private CanvasGroup group;
        private CardDisplay card;

        private string currentLanguage;


        // Start is called before the first frame update
        void Start()
        {
            group = inspectMenu.GetComponent<CanvasGroup>();
            currentLanguage = I2.Loc.LocalizationManager.CurrentLanguage;
        }

        // Update is called once per frame
        void Update()
        {
            if (isInspecting)
            {
                if (isPaused)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }

        public void Enter(CardDisplay c, Card cardInfo)
        {
            isInspecting = true;
            isPaused = true;

            inspectMenu.SetActive(true);
            group.DOFade(1f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true);

            card = Instantiate(c, inspectMenu.transform, false);
            Material mat = Instantiate(card.cardImage.material);
            card.cardImage.material = mat;
            card.SetCard(cardInfo);

            if (card.GetComponent<Selectable>())
            {
                card.GetComponent<Selectable>().enabled = false;
            }

            foreach (TextMeshProUGUI text in card.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.fontSizeMax = 200;
            }

            foreach (Image image in card.GetComponentsInChildren<Image>())
            {
                image.material.SetFloat("_Saturation", 1f);
                image.material.SetFloat("_OutlineStrength", 0f);
            }

            var rectTransform = card.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            rectTransform.anchoredPosition = cardPos;

            rectTransform.sizeDelta = cardSize;


            string description = 
                currentLanguage.Equals("Chinese") ? card.card.description : card.card.descriptionEn;
            inspectMenu.GetComponentInChildren<TextMeshProUGUI>().text = description;

                GlobalAudioManager.Instance.Play("Inspect", Vector3.zero);
            Time.timeScale = 0f;
        }

        public void Exit()
        {
            if (card != null)
            {
                Destroy(card.gameObject);
            }

            isInspecting = false;
            isPaused = false;
            Time.timeScale = 1f;

            group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
                .OnComplete(() => inspectMenu.SetActive(false));
        }
    }
}