using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

public class CardPlayer : Singleton<CardPlayer>
{ 
    public Card cardPlaying { get; private set; }
    public SplatManager splat { get; private set; }

    private PlayerController player;
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
            && hit.transform.tag == "Floor")
        {

            if (Input.GetMouseButtonDown(0) 
                && !player.isCasting
                && !EventSystem.current.IsPointerOverGameObject())
            {
                ConfirmCard();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelCard();
            }
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
            if (resource.isResourceEnough(cardPlaying.primaryChange,
                                        cardPlaying.secondaryChange))
            {
                isPlayingCard = true;
                player.isSelected = false;

                cardPlaying.Ready();

                return true;
            }
            else
            {
                isPlayingCard = false;
                splat.CancelSpellIndicator();
                splat.SelectRangeIndicator(cardPlaying.owner + "Range");

                player.isSelected = false;

                return false;
            }
        }
    }

    private void ConfirmCard()
    {
        resource.changeResource(cardPlaying.primaryChange,
                                cardPlaying.secondaryChange);

        isPlayingCard = false;
        player.isSelected = false;
        player.setIsCasting(true);
        player.spriteRenderer.flipX = splat.Get3DMousePosition().x 
                                      < player.gameObject.transform.position.x;
        Invoke("resumeAction", cardPlaying.castTime);

        cardPlaying.Play();
        DeckManager.Instance.DiscardCard(cardPlaying);
        
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
        splat.SelectRangeIndicator(cardPlaying.owner +"Range");
    }

    private void resumeAction()
    {
        player.setIsCasting(false);
    }
}
