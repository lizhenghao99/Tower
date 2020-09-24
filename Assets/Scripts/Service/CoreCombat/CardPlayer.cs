using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;
using ProjectTower;

namespace ProjectTower
{
    public class CardPlayer : Singleton<CardPlayer>
    {
        public Card cardPlaying { get; private set; }
        public SplatManager splat { get; private set; }

        public PlayerController player { get; private set; }
        private PlayerResource resource;
        private LayerMask layerMask;
        public bool isPlayingCard { get; private set; } = false;
        // Start is called before the first frame update
        void Start()
        {
            layerMask = LayerMask.GetMask("Environment");
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (isPlayingCard && Physics.Raycast(ray, out hit, 1000, layerMask)
                && (hit.transform.tag == "Floor" || cardPlaying is Buff))
            {

                if (Input.GetMouseButtonDown(0)
                    && !player.isCasting
                    && !EventSystem.current.IsPointerOverGameObject())
                {
                    ConfirmCard();
                }
            }
            if (isPlayingCard && Input.GetMouseButtonDown(1))
            {
                CancelCard();
            }
            if (isPlayingCard && player.GetComponent<Health>().isDead)
            {
                CancelCard();
                splat.CancelRangeIndicator();
            }
        }

        public bool Play(Card card)
        {
            cardPlaying = card;
            player = FindObjectsOfType<PlayerController>()
                    .Where(player => player.name == cardPlaying.owner.ToString())
                    .FirstOrDefault();
            splat = player.GetComponentInChildren<SplatManager>();
            resource = player.GetComponent<PlayerResource>();

            if (player.isCasting)
            {
                return false;
            }
            else
            {
                foreach (PlayerController p in FindObjectsOfType<PlayerController>())
                {
                    p.isSelected = false;
                }
                if (resource.IsResourceEnough(cardPlaying.primaryChange,
                                            cardPlaying.secondaryChange))
                {
                    isPlayingCard = true;

                    cardPlaying.Ready();

                    return true;
                }
                else
                {
                    isPlayingCard = false;
                    splat.CancelSpellIndicator();
                    splat.SelectRangeIndicator(cardPlaying.owner + "Range");

                    return false;
                }
            }
        }

        private void ConfirmCard()
        {
            resource.ChangeResource(cardPlaying.primaryChange,
                                    cardPlaying.secondaryChange);

            isPlayingCard = false;
            player.isSelected = false;
            player.setIsCasting(true);

            if (!(cardPlaying is Buff))
            {
                player.spriteRenderer.flipX = splat.Get3DMousePosition().x
                                         < player.gameObject.transform.position.x;
            }

            Resume(player, cardPlaying.castTime);

            if (cardPlaying.specialPrefab != null)
            {
                var special = Instantiate(
                    cardPlaying.specialPrefab,
                    cardPlaying.specialPrefab.transform.position,
                    cardPlaying.specialPrefab.transform.rotation);
                special.SetCard(cardPlaying);
                special.transform.position += splat.GetSpellCursorPosition();
            }
            cardPlaying.Play();
            player.animator.SetTrigger(cardPlaying.animationTrigger);
            GlobalAudioManager.Instance.Play("Place", Vector3.zero);
            GlobalAudioManager.Instance.Play(
                cardPlaying.sfx, player.transform.position);
            FindObjectOfType<DeckManager>().DiscardCard(cardPlaying);

            splat.CancelSpellIndicator();
            splat.SelectRangeIndicator(cardPlaying.owner + "Range");

            EventSystem.current.SetSelectedGameObject(null);
        }

        private void CancelCard()
        {
            player.isSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
            isPlayingCard = false;
            splat.CancelSpellIndicator();
            splat.SelectRangeIndicator(cardPlaying.owner + "Range");
            GlobalAudioManager.Instance.Play("Cancel", Vector3.zero);
        }

        private void Resume(PlayerController p, float time)
        {
            StartCoroutine(Utils.Timeout(() =>
            {
                p.setIsCasting(false);
            }, time));
        }
    }
}