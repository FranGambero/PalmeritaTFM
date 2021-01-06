using System.Collections.Generic;
using UnityEngine;

public class LogrosPanel : MonoBehaviour {
    public GameObject[] logroTicks;
    public List<GameObject> paneles;

    private void Awake() {
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++) {
            paneles.Add(transform.GetChild(childIndex).gameObject);
        }
    }

    public void ChangeAchievementTicks(LevelData newLevelData) {
        for (int i = 0; i < logroTicks.Length; i++) {
            Debug.LogWarning("SACO LOGRO " + i + " - " + newLevelData.logros.Length);
            if (i < newLevelData.logros.Length) {
                paneles[i].SetActive(true);
                logroTicks[i].SetActive(newLevelData.logros[i].done);
            } else {
                paneles[i].SetActive(false);
                //logroTicks[i].transform.parent.gameObject.SetActive(false);
            }

        }
    }

    public void GetLogritos(int level) {
        int currentZone = PlayerPrefs.GetInt("CurrentZone");
        LevelData levelData = MapamundiManager.Instance.GetCurrentLevel(currentZone, level);
        ChangeAchievementTicks(levelData);
    }
}
