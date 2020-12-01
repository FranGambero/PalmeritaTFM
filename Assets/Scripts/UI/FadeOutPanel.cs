using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeOutPanel : MonoBehaviour {
    public MenuButton BtnTrigger { get; set; }
    public string fadeOutAnimName = "FadeOut";
    public void FadeOut() {
       // gameObject.SetActive(true);
        float time = .2f;
        if (GetComponent<Animator>().runtimeAnimatorController.animationClips.Any(x => x.name == fadeOutAnimName))
            time = GetComponent<Animator>().runtimeAnimatorController.animationClips.First(x => x.name == fadeOutAnimName).length;
        BtnTrigger.animationLength = time;
        GetComponent<Animator>().Play(fadeOutAnimName);
    }
}
