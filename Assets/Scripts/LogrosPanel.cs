using UnityEngine;

public class LogrosPanel : MonoBehaviour {
    public GameObject[] logroTicks;

    public void ChangeAchievementTicks(LevelData newLevelData) {
        for (int i = 0; i < logroTicks.Length; i++) {
            logroTicks[i].SetActive(newLevelData.logros[i].done);
        }
    }

    public void GetLogritos(int level) {
        int currentZone = PlayerPrefs.GetInt("CurrentZone");
        LevelData levelData = MapamundiManager.Instance.GetCurrentLevel(currentZone, level);
        ChangeAchievementTicks(levelData);
    }
}
