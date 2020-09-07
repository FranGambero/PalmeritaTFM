using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : Button
{
    public Image indicatorImg;
    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        indicatorImg.enabled = true;
    }
    public override void OnDeselect(BaseEventData eventData) {
        base.OnDeselect(eventData);
        indicatorImg.enabled = false;
    }
}
