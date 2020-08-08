using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TowerUtils;
using DG.Tweening;

public class DiscardButton : Selectable
{
    public Card.Owner owner;

    private PlayerController player;
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
            gameObject.transform.DORotate(
            new Vector3(0, 0, 20), 0.3f)
            .SetEase(Ease.OutQuint);
        }   
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!(EventSystem.current.currentSelectedGameObject == gameObject)
            && IsInteractable())
        {
            base.OnPointerExit(eventData);
            gameObject.transform.DORotate(
                new Vector3(0, 0, 0), 0.3f)
                .SetEase(Ease.OutQuint);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsInteractable())
        {
            base.OnPointerDown(eventData);
            isDiscarding = true;
            gameObject.transform.DOPunchRotation(
                new Vector3(0, 0, 45), 0.5f)
                .SetEase(Ease.OutQuint);


            switch (owner)
            {
                case Card.Owner.Luban:
                    GlobalAudioManager.Instance.Play("Hammer", Vector3.zero);
                    break;
                case Card.Owner.Daoshi:
                    break;
                case Card.Owner.thirdChar:
                    break;
                default:
                    break;
            }
        }  
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        gameObject.transform.DORotate(
                new Vector3(0, 0, 0), 0.3f)
                .SetEase(Ease.OutQuint);
    }


    public bool Discard(Card card)
    {
        isDiscarding = false;
        FindObjectOfType<DeckManager>().DiscardCard(card);
        
        switch (owner)
        {
            case Card.Owner.Luban:
                player.setIsCasting(true);
                StartCoroutine(Utils.Timeout(() =>
                    player.setIsCasting(false), 10f));
                break;
            case Card.Owner.Daoshi:
                player.GetComponent<DaoshiResource>()
                    .ChangeResource(card.primaryChange / 2, -1);
                break;
            case Card.Owner.thirdChar:
                break;
            default:
                break;
        }
        
        return true;
    }
}
