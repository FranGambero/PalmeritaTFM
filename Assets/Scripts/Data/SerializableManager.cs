using System.IO;
using UnityEngine;
using ElJardin;

public class SerializableManager : Singleton<SerializableManager> {

    public string preBuildPath = "Assets/Resources/ZoneData";
    public string[] zoneFile = { "/Zone0", "/Zone1", "/Zone2", "/Zone3" };
    public string postBuildPath = "ElJardin_Data/Resources";
    public string format = ".json";
    public string[] zonenPrefab = {
    "{\"zoneId\":0,\"zoneName\":\"Primavera\",\"levels\":[{\"levelName\":\"Nivel 1\",\"id\":0,\"zone\":0,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 2\",\"id\":1,\"zone\":0,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 3\",\"id\":2,\"zone\":0,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 4\",\"id\":3,\"zone\":0,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 5\",\"id\":3,\"zone\":0,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]}]}",

    "{\"zoneId\":1,\"zoneName\":\"Verano\",\"levels\":[{\"levelName\":\"Nivel 6\",\"id\":0,\"zone\":1,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 7\",\"id\":1,\"zone\":1,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 8\",\"id\":2,\"zone\":1,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 9\",\"id\":3,\"zone\":1,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]}]}",

    "{\"zoneId\":2,\"zoneName\":\"Otoño\",\"levels\":[{\"levelName\":\"Nivel 10\",\"id\":0,\"zone\":2,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 11\",\"id\":1,\"zone\":2,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 12\",\"id\":2,\"zone\":2,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 13\",\"id\":3,\"zone\":2,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]}]}",

    "{\"zoneId\":3,\"zoneName\":\"Invierno\",\"levels\":[{\"levelName\":\"Nivel 14\",\"id\":0,\"zone\":3,\"isCompleted\":true,\"logros\":[{\"achievementName\":\"Completado\",\"done\":true,\"animationDone\":true},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":true},{\"achievementName\":\"Movimientos\",\"done\":true,\"animationDone\":false}]},{\"levelName\":\"Nivel 15\",\"id\":1,\"zone\":3,\"isCompleted\":true,\"logros\":[{\"achievementName\":\"Completado\",\"done\":true,\"animationDone\":true},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":true},{\"achievementName\":\"Movimientos\",\"done\":true,\"animationDone\":false}]},{\"levelName\":\"Nivel 16\",\"id\":2,\"zone\":3,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]},{\"levelName\":\"Nivel 17\",\"id\":3,\"zone\":3,\"isCompleted\":false,\"logros\":[{\"achievementName\":\"Completado\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Flores\",\"done\":false,\"animationDone\":false},{\"achievementName\":\"Movimientos\",\"done\":false,\"animationDone\":false}]}]}"
};
    public ZoneData DeSerializeZone(int zoneId) {
        ZoneData zoneData;
        string path = GetZonePath(zoneId);
        if (!File.Exists(path)) {
            using (StreamWriter sw = new StreamWriter(path)) {
                sw.Write(zonenPrefab[zoneId]);
            }
        }
        zoneData = DeSerializeSelectedZone(path);

        return zoneData;
    }
    public ZoneData SerializeZone(ZoneData zoneData) {
        string path = GetZonePath(zoneData.zoneId);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(JsonUtility.ToJson(zoneData));
        writer.Close();

        return zoneData;
    }
    private ZoneData DeSerializeSelectedZone(string path) {

        ZoneData zoneData = null;
        
        if (File.Exists(path)) {

            StreamReader reader = new StreamReader(path);
            zoneData = JsonUtility.FromJson<ZoneData>(reader.ReadToEnd());
            reader.Close();
        }

        return zoneData;
    }

    public string GetZonePath(int zoneId) {
        string path = null;
#if UNITY_EDITOR
        path = preBuildPath;
#else
        path = Application.dataPath;
#endif
        path += zoneFile[zoneId];
        path += format;

        return path;
    }

}
