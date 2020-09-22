using System.Collections;
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
    bool clicked=false;

    public void OnSelect(BaseEventData eventData) {
        indicatorImg.enabled = true;
    }
    public void OnDeselect(BaseEventData eventData) {
        indicatorImg.enabled = false;
    }

    public void OnClick(BaseEventData eventData) {
        if (!clicked) {
            clicked = true;
        indicatorImg.GetComponent<Animator>().Play(playAnimation);
        StartCoroutine(PlayEvent());
        }
    }
    IEnumerator PlayEvent() {
        Debug.Log(indicatorImg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForFixedUpdate();

        float time = indicatorImg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Debug.Log(indicatorImg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(time);
        OnClickEvent?.Invoke();
        clicked = false;
    }
}
