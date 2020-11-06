using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapamundiManager : MonoBehaviour
{
    public ZoneData[] zoneDataArray;
    private int numZones;
    private int currentZone;

    private void Awake() {
        numZones = 4;
        zoneDataArray = new ZoneData[numZones];
        currentZone = PlayerPrefs.GetInt("CurrentZone", 0);
    }

    private void Start() {
        SetCurrentZone();
    }

    public void SetCurrentZone() {
        zoneDataArray[currentZone] = SerializableManager.Instance.DeSerializeZone(currentZone);
    }

    [ContextMenu("Guarda Carla")]
    public void SaveZoneData() {
        SerializableManager.Instance.SerializeZone(zoneDataArray[0]);
    }
}
