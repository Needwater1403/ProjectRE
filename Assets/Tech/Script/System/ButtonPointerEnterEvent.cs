using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPointerEnterEvent : MonoBehaviour
{
    public EventTrigger _eventTrigger;
    void Start()
    {
        var eventPointerEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        eventPointerEnter.callback.AddListener(OnButtonPointerEnter_Event);
        
        var eventSelect= new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Select
        };
        eventPointerEnter.callback.AddListener(OnButtonPointerEnter_Event);
        
        _eventTrigger.triggers.Add(eventPointerEnter);
        _eventTrigger.triggers.Add(eventSelect);
    }

    private void OnButtonPointerEnter_Event(BaseEventData eventData)
    {
        OnButtonPointerEnter();
    }

    private void OnButtonPointerEnter()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
