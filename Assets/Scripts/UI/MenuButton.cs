using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(EventTrigger))]
public class MenuButton : MonoBehaviour {
    public Image indicatorImg;
    public float animationLength = .2f;
    private string playAnimation;
    public System.Action OnClickEvent;
    public System.Action OnPreAnimationEvent;
    bool clicked = false;

    public string AnimationName { get => playAnimation; set => playAnimation = value; }

    public void OnSelect(BaseEventData eventData) {
        AkSoundEngine.PostEvent("UI_Cursor_In", gameObject);
        if (indicatorImg)
            indicatorImg.enabled = true;
    }
    public void OnDeselect(BaseEventData eventData) {
        if (indicatorImg)
            indicatorImg.enabled = false;
    }

    public void OnClick(BaseEventData eventData) {
        if (!clicked) {
            clicked = true;
            OnPreAnimationEvent?.Invoke();
            if (indicatorImg)
                indicatorImg.GetComponent<Animator>().Play(AnimationName);
            StartCoroutine(PlayEvent());
        }
    }

    IEnumerator PlayEvent() {

        yield return new WaitForSeconds(animationLength);
        OnClickEvent?.Invoke();
        clicked = false;
    }
}
