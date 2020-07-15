using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] public Card.Owner owner;

    private PlayerController player;
    private HandManager hand;
    private Healthbar healthbar;
    private ResourceDisplay resource;
    private DiscardButton discardButton;
    private HeadButton headButton;
    private PlayerHealth health;

    private bool drawerDead = false;

    private void Awake()
    {
        player = FindObjectsOfType<PlayerController>()
                        .Where(p => p.gameObject.name == owner.ToString())
                        .FirstOrDefault();
        hand = GetComponentInChildren<HandManager>();
        healthbar = GetComponentInChildren<Healthbar>();
        resource = GetComponentInChildren<ResourceDisplay>();
        discardButton = GetComponentInChildren<DiscardButton>();
        headButton = GetComponentInChildren<HeadButton>();
        health = player.GetComponent<PlayerHealth>();
        

        hand.player = player;
        healthbar.player = player;
        resource.player = player;
        discardButton.owner = owner;
        headButton.owner = owner;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        discardButton.interactable = 
            !(CardPlayer.Instance.isPlayingCard 
            || player.isCasting 
            || health.isDead);

        headButton.interactable =
            !(CardPlayer.Instance.isPlayingCard || health.isDead);

        if (health.isDead)
        {
            if (!drawerDead)
            {
                foreach (CardClick c in GetComponentsInChildren<CardClick>())
                {
                    c.SetInteractable(false);
                }
            }
            drawerDead = true;
        }
    }
}
