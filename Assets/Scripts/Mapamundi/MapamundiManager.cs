﻿using ElJardin;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapamundiManager : Singleton<MapamundiManager> {
    public ZoneData[] zoneDataArray;
    private int numZones;
    public int currentZone, currentLevel;
    public int currentPetals;

    public GameObject[] levelZonePanels;
    public TextMeshProUGUI zoneTextTag, petalsTextTag;
    public Action<int> onZoneChange;

    public FadeOutPanel fadeOutPanel;

    private void Start() {
        Init();
        SetAllZones();
        CountCurrentPetals();
        if (fadeOutPanel) {
            AkSoundEngine.PostEvent("UI_Trans_2_In", gameObject);
            fadeOutPanel.FadeIn();
        }
    }
    public void Init() {
        numZones = 4;
        zoneDataArray = new ZoneData[numZones];
        currentZone = PlayerPrefs.GetInt(Keys.Scenes.CURRENT_ZONE, 0);
        /////
        //currentZone = 0;
        //PlayerPrefs.SetInt(Keys.Scenes.CURRENT_ZONE, 0);
        /////
        currentLevel = PlayerPrefs.GetInt(Keys.Scenes.CURRENT_LEVEL, 0);

        if (levelZonePanels.Length > 0)
            levelZonePanels[currentZone].SetActive(true);

        transform.SetParent(null);
        // DontDestroyOnLoad(this.gameObject);
    }

    public void ChargeZone(int zoneId) {
        zoneDataArray[zoneId] = SerializableManager.Instance.DeSerializeZone(zoneId);
    }

    public void SetAllZones() {
        Debug.LogWarning("CARGO LAS ZONAS");
        for (int zoneId = 0; zoneId < zoneDataArray.Length; zoneId++) {
            ChargeZone(zoneId);
        }
    }

    public ZoneData GetCurrentZone(int zoneId) {
        if (zoneDataArray[zoneId] == null)
            ChargeZone(zoneId);
        return zoneDataArray[zoneId];
    }

    public LevelData GetCurrentLevel(int zoneId, int levelId) {
        return GetCurrentZone(zoneId).levels[levelId];
    }

    public LevelData GetCurrentLevel(int levelId) {
        return GetCurrentZone(currentZone).levels[levelId];
    }

    [ContextMenu("Guarda Carla")]
    public void SaveZoneData() {
        SerializableManager.Instance.SerializeZone(zoneDataArray[0]);
        SerializableManager.Instance.SerializeZone(zoneDataArray[1]);
        //for (int zoneId = 0; zoneId < zoneDataArray.Length; zoneId++) {
        //    SerializableManager.Instance.SerializeZone(zoneDataArray[zoneId]);
        //}
    }

    public void SaveLevel(LevelData newLevelData) {
        //ZoneData zoneData = GetCurrentZone(currentZone);
        if (zoneDataArray.Length > 0) {
            zoneDataArray[currentZone].levels[currentLevel] = newLevelData;
            SaveZoneData();

        }
    }

    public void CountCurrentPetals() {
        ZoneData zoneData = GetCurrentZone(currentZone);
        int totalPetals = CountTotalPetals(zoneData);
        this.currentPetals = 0;

        for (int i = 0; i < zoneData.levels.Length; i++) {
            //Miro en cada nivel de la zona sus logros
            LevelData m_level = zoneData.levels[i];
            for (int j = 0; j < m_level.logros.Length; j++) {
                // Checkeo todos los logros de cada nivel
                if (m_level.logros[j].done)
                    currentPetals++;
            }
        }

        string petalsText = currentPetals + " / " + totalPetals;
        //string zoneText = "Zona " + currentZone;
        string zoneText = GetCurrentZone(currentZone).zoneName;
        if (petalsTextTag) {
            petalsTextTag.text = petalsText;
            zoneTextTag.text = zoneText;
        }
    }

    private int CountTotalPetals(ZoneData currentZoneData) {
        int totalPetals = 0;
        foreach (var level in currentZoneData.levels) {
            totalPetals += level.logros.Length;
        }

        return totalPetals;
    }

    public void ChangeZone(bool greater) {
        int avanze = 1;
        levelZonePanels[currentZone].SetActive(false);

        if (!greater)
            avanze = -1;

        //currentZone = (currentZone + avanze) % 2;
        currentZone += avanze;
        if (currentZone >= levelZonePanels.Length - 1) {
            currentZone = 0;
        } else if (currentZone < 0) {
            currentZone = 1;
        }
        PlayerPrefs.SetInt(Keys.Scenes.CURRENT_ZONE, currentZone);
        levelZonePanels[currentZone].SetActive(true);
        onZoneChange?.Invoke(currentZone);

        CountCurrentPetals();

    }
}
