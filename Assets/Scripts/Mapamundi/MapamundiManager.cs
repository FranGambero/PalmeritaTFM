using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapamundiManager : Singleton<MapamundiManager> {
    public ZoneData[] zoneDataArray;
    private int numZones;
    public int currentZone, currentLevel;

    private void Awake() {
        numZones = 4;
        zoneDataArray = new ZoneData[numZones];
        currentZone = PlayerPrefs.GetInt("CurrentZone", 0);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        SetCurrentZone();
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

    [ContextMenu("Guarda Carla")]
    public void SaveZoneData() {
        SerializableManager.Instance.SerializeZone(zoneDataArray[currentZone]);
    }

    public void SaveLevel(LevelData newLevelData) {
        //ZoneData zoneData = GetCurrentZone(currentZone);
        zoneDataArray[currentZone].levels[currentLevel] = newLevelData;
        SaveZoneData();
    }
}
