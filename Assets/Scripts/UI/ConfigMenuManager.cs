using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenuManager : MonoBehaviour
{
    public MenuButton backBtn;

    private void Awake() {
        backBtn.OnClickEvent += CloseCongifMenu;
    }

    private void CloseCongifMenu() {
        gameObject.SetActive(false);
    }
}
