using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowManager : MonoBehaviour
{
    public int minPetals;
    private Button myButton;
    public GameObject nextPanel;

    private void Awake() {
        myButton = GetComponent<Button>();
    }

    private void Start() {
        CheckLeftPetals();
        CheckChangeZone();
    }

    private void CheckLeftPetals() {

        nextPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Consigue " + (minPetals - MapamundiManager.Instance.currentPetals) + " petalos más para avanzar";
    }

    public void CheckChangeZone() {
        if(MapamundiManager.Instance.currentPetals >= minPetals) {
            myButton.interactable = true;
            nextPanel.SetActive(false);
        }
    }
}
