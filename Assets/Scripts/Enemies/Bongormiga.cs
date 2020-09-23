using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bongormiga : MonoBehaviour, ITurn {
    public Animator myAnimator;
    public int myTurnIndex;

    public int turnIndex { get => myTurnIndex; set => myTurnIndex = value; }

    private void Awake() {
        myAnimator = GetComponentInChildren<Animator>();
    }

    public void onTurnStart(int currentIndex) {
        if (turnIndex == currentIndex) {
            Debug.Log("TOCO EL COSO");
            myAnimator.Play("PlayBongo0");
            //AkSoundEngine.PostEvent("Hormiga_Ataque_In", gameObject);
            Invoke(nameof(onTurnFinished), 3f);
        }
    }

    public void onTurnFinished() {
        Semaphore.Instance.onTurnEnd(turnIndex);
    }




}
