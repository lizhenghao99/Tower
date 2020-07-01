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

    private void Awake()
    {
        player = FindObjectsOfType<PlayerController>()
                        .Where(p => p.gameObject.name == owner.ToString())
                        .FirstOrDefault();
        hand = GetComponentInChildren<HandManager>();
        healthbar = GetComponentInChildren<Healthbar>();
        resource = GetComponentInChildren<ResourceDisplay>();
        

        hand.player = player;
        healthbar.player = player;
        resource.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
