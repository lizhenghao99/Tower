using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ProjectTower
{
    public class DeckBuilderSelection : MonoBehaviour
    {
        [SerializeField] DeckBuilder deckBuilder;
        [SerializeField] Card.Owner[] owners;
        [SerializeField] GameObject[] collectionCardPrefabs;
        [SerializeField] private GameObject loadingScreen;

        public void Enter()
        {
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>()
                .DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
        }

        public void Exit()
        {
            StartCoroutine(Utils.LoadAsync(1, loadingScreen));
            GetComponent<CanvasGroup>()
                .DOFade(0f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public void SelectCharacter(int index)
        {
            deckBuilder.gameObject.SetActive(true);
            deckBuilder.SetOwner(owners[index], collectionCardPrefabs[index]);
            deckBuilder.GetComponent<CanvasGroup>()
                .DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
        }
    }
}