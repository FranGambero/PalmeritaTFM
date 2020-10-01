﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(EventTrigger))]
public class MenuButton : MonoBehaviour {
    public Image indicatorImg;
    public float cacafuti;
    public string playAnimation;
    public System.Action OnClickEvent;
    public System.Action OnPreAnimationEvent;
    bool clicked = false;

    public void OnSelect(BaseEventData eventData) {
        AkSoundEngine.PostEvent("UI_Cursor_In", gameObject);
        indicatorImg.enabled = true;
    }
    public void OnDeselect(BaseEventData eventData) {
        indicatorImg.enabled = false;
    }

    public void OnClick(BaseEventData eventData) {
        if (!clicked) {
            clicked = true;
            OnPreAnimationEvent?.Invoke();
            indicatorImg.GetComponent<Animator>().Play(playAnimation);
            StartCoroutine(PlayEvent());
        }
    }

    IEnumerator PlayEvent() {
        yield return new WaitForFixedUpdate();

        float time = indicatorImg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time - 0.2f);
        OnClickEvent?.Invoke();
        clicked = false;
    }
}
