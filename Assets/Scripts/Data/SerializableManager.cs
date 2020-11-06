using System.IO;
using UnityEngine;
using ElJardin;

public class SerializableManager : Singleton<SerializableManager> {

    public string preBuildPath = "Assets/Resources/ZoneData";
    public string[] zoneFile = { "/Zone0", "/Zone1", "/Zone2", "/Zone3" };
    public string postBuildPath = "PalmeritaTFM_Data/Resources";
    public string format = ".json";

    public ZoneData DeSerializeZone(int zoneId) {
        ZoneData zoneData;
        string path = GetZonePath(zoneId);

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

    private string GetZonePath(int zoneId) {
        string path = null;
#if UNITY_STANDALONE
        path = postBuildPath;
#endif
#if UNITY_EDITOR
        path = preBuildPath;
#endif

        path += zoneFile[zoneId];
        path += format;

        return path;
    }

}
