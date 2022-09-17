using UnityEngine;

public class WeatherEffect {
    public static SkyPrefs[] skyList = new SkyPrefs[]{
        // Blue sky and white clouds
        new SkyPrefs() { mapfile = "airplane.rsw"    , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "airplane_01.rsw" , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "gonryun.rsw"     , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "gon_dun02.rsw"   , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "himinn.rsw"      , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "ra_temsky.rsw"   , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "rwc01.rsw"       , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "sch_gld.rsw"     , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "valkyrie.rsw"    , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        new SkyPrefs() { mapfile = "yuno.rsw"        , skyColor = new Color(0.4f, 0.6f, 0.8f, 1.0f), cloudColor = new Color(1.0f, 1.0f, 1.0f, 1.0f)},
        //others
        new SkyPrefs() { mapfile = "5@tower.rsw"     , skyColor = new Color(0.2f, 0.0f, 0.2f, 1.0f), cloudColor = new Color(1.0f, 0.7f, 0.7f, 1.0f)},
        new SkyPrefs() { mapfile = "thana_boss.rsw"  , skyColor = new Color(0.88f, 0.83f, 0.76f, 1.0f), cloudColor = new Color(0.37f, 0.0f, 0.0f, 1.0f)},
    };

    public static SkyPrefs GetPrefs(string mapfile) {
        foreach (SkyPrefs pref in skyList) {
            if (pref.mapfile.Equals(mapfile)) {
                return pref;
            }
        }

        return null;
    }

    public static bool HasMap(string mapfile) {
        foreach (SkyPrefs pref in skyList) {
            if (pref.mapfile.Equals(mapfile)) {
                return true;
            }
        }

        return false;
    }

    public class SkyPrefs {
        public string mapfile;
        public Color32 skyColor;
        public Color32 cloudColor;
    }
}
