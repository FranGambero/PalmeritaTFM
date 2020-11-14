using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapamundiManager : Singleton<MapamundiManager> {
    public ZoneData[] zoneDataArray;
    private int numZones;
    public int currentZone, currentLevel;
    public int currentPetals;

    public TextMeshProUGUI petalsTextTag;

    private void Awake() {
        numZones = 4;
        zoneDataArray = new ZoneData[numZones];
        currentZone = PlayerPrefs.GetInt("CurrentZone", 0);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        SetCurrentZone();
        CountCurrentPetals();
    }

    public void SetCurrentZone() {
        zoneDataArray[currentZone] = SerializableManager.Instance.DeSerializeZone(currentZone);
    }

    public ZoneData GetCurrentZone(int zoneId) {
        return SerializableManager.Instance.DeSerializeZone(zoneId);
    }

    public LevelData GetCurrentLevel(int zoneId, int levelId) {
        return GetCurrentZone(zoneId).levels[levelId];
    }

    public LevelData GetCurrentLevel(int levelId) {
        return GetCurrentZone(currentZone).levels[levelId];
    }

    [ContextMenu("Guarda Carla")]
    public void SaveZoneData() {
        SerializableManager.Instance.SerializeZone(zoneDataArray[currentZone]);
    }

    public void SaveLevel(LevelData newLevelData) {
        //ZoneData zoneData = GetCurrentZone(currentZone);
        zoneDataArray[currentZone].levels[currentLevel] = newLevelData;
        SaveZoneData();
    }

    public void CountCurrentPetals() {
        ZoneData zoneData = GetCurrentZone(currentZone);
        int totalPetals = zoneData.levels.Length * 3; // 3 logros por nivel
        int currentPetals = 0;

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

        petalsTextTag.text = petalsText;

    }
}
