using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    private HandManager[] hands;

    private void Awake()
    {
        hands = FindObjectsOfType<HandManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCombat();
        }
    }

    public void StartCombat()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Minion"),
                                     LayerMask.NameToLayer("Minion"),
                                     true);
        DeckManager.Instance.StartCombat();
        foreach (HandManager h in hands)
        {
            h.StartCombat();
        }
    }
}
