using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Keys {
    public struct Volume {
        public static string PREF_VOL_MUSIC = "PrefVol_Music";
        public static string PREF_VOL_SFX = "PrefVol_SFX";
    }

    public struct WWise {
        public static string RTPC_Music = "Vol_Musica";
        public static string RTPC_SFX = "Vol_SFX";
        public static string RTPC_LPF = "LPF_Musica";
    }

    public struct Scenes {
        public static string LOAD_SCENE_INT= "NextLevel";
        public static string LOAD_SCENE_STRING= "NextLevelString";
        public static string LAST_PLAYED_LEVEL = "LastPlayeLevel";
    }
}
