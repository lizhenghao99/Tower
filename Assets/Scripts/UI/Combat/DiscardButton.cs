using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TowerUtils;
using DG.Tweening;

public abstract class DiscardButton : Selectable
{
    public Card.Owner owner;

    protected PlayerController player;
    public bool isDiscarding = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = FindObjectsOfType<PlayerController>()
           .Where(p => p.gameObject.name == owner.ToString())
           .FirstOrDefault();
    }

    private void Update()
    {
        if (isDiscarding)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isDiscarding = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (IsInteractable())
        {
            base.OnPointerEnter(eventData);
            PointerEnterBehavior();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!(EventSystem.current.currentSelectedGameObject == gameObject)
            && IsInteractable())
        {
            base.OnPointerExit(eventData);
            PointerExitBehavior();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsInteractable())
        {
            base.OnPointerDown(eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);
            isDiscarding = true;
            PointerDownBehavior();
        }  
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        DeselecteBehavior();
    }

    public bool Discard(Card card)
    {
        isDiscarding = false;
        FindObjectOfType<DeckManager>().DiscardCard(card);
        
        return DiscardBehavior(card);
    }

    protected abstract void PointerDownBehavior();

    protected abstract void PointerEnterBehavior();

    protected abstract void PointerExitBehavior();

    protected abstract void DeselecteBehavior();

    protected abstract bool DiscardBehavior(Card card);
}
