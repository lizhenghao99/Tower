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
    private LayerMask layerMask;
    private bool isPlayingCard = false;
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

    public void Play(Card card)
    {
        cardPlaying = card;
        player = FindObjectsOfType<PlayerController>()
                .Where(player => player.name == cardPlaying.owner.ToString())
                .FirstOrDefault();
        splat = player.GetComponentInChildren<SplatManager>();


        if (!player.isCasting)
        {
            isPlayingCard = true;

            cardPlaying.Ready();
        }
    }

    private void ConfirmCard()
    {
        isPlayingCard = false;
        player.isCasting = true;
        player.spriteRenderer.flipX = splat.Get3DMousePosition().x 
                                      < player.gameObject.transform.position.x;
        Invoke("resumeAction", cardPlaying.castTime);

        cardPlaying.Play();
        
        splat.CancelSpellIndicator();
        splat.SelectRangeIndicator(cardPlaying.owner + "Range");

    }

    private void CancelCard()
    {
        isPlayingCard = false;
        splat.CancelSpellIndicator();
        splat.SelectRangeIndicator(cardPlaying.owner +"Range");
    }

    private void resumeAction()
    {
        player.isCasting = false;
    }
}
