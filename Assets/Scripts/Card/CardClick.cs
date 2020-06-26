using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour
{
    [SerializeField] Card card;
    private EventTrigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        // add event handler
        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(
            (data) => OnPointerClick((PointerEventData) data));
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData data)
    {
        CardPlayer.Instance.Play(card);
    }
}
