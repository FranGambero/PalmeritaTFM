using ElJardin;
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

    private void Awake() {
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

    private void Start() {
        SetAllZones();
        CountCurrentPetals();
        if (fadeOutPanel) {
            AkSoundEngine.PostEvent("UI_Trans_2_In", gameObject);
            fadeOutPanel.FadeIn();
        }
    }

    public void SetCurrentZone(int zoneId) {
        zoneDataArray[zoneId] = SerializableManager.Instance.DeSerializeZone(zoneId);
    }

    public void SetAllZones() {
        Debug.LogWarning("CARGO LAS ZONAS");
        for (int zoneId = 0; zoneId < zoneDataArray.Length; zoneId++) {
            SetCurrentZone(zoneId);
        }
    }

    public ZoneData GetCurrentZone(int zoneId) {
        if (zoneDataArray[zoneId] == null)
            SetCurrentZone(zoneId);
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
        Debug.LogWarning("VOY A GUARDARRRRR CON ZONA " + currentZone + " y level " + currentLevel);
        Debug.LogWarning("LA LENGTH " + zoneDataArray.Length);
        if (zoneDataArray.Length > 0) {
            zoneDataArray[currentZone].levels[currentLevel] = newLevelData;
            SaveZoneData();

        }
    }

    public void CountCurrentPetals() {
        Debug.LogWarning("Voy a coger petalitos con " + currentZone);
        ZoneData zoneData = GetCurrentZone(currentZone);
        int totalPetals = zoneData.levels.Length * 3; // 3 logros por nivel
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
            Debug.LogWarning("Vas a meter la zonita  text : " + currentZone + " === " + zoneTextTag.text);
        }
    }

    public void ChangeZone(bool greater) {
        int avanze = 1;
        levelZonePanels[currentZone].SetActive(false);

        if (!greater)
            avanze = -1;

        //currentZone = (currentZone + avanze) % 2;
        currentZone += avanze;
        if (currentZone >= levelZonePanels.Length-1) {
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
