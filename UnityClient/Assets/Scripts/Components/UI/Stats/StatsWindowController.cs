using UnityEngine;
using TMPro;
using System;

public class StatsWindowController : DraggableUIWindow {

    [SerializeField] private TextMeshProUGUI Str;
    [SerializeField] private TextMeshProUGUI StrNeed;

    [SerializeField] private TextMeshProUGUI Agi;
    [SerializeField] private TextMeshProUGUI AgiNeed;

    [SerializeField] private TextMeshProUGUI Vit;
    [SerializeField] private TextMeshProUGUI VitNeed;

    [SerializeField] private TextMeshProUGUI Int;
    [SerializeField] private TextMeshProUGUI IntNeed;

    [SerializeField] private TextMeshProUGUI Dex;
    [SerializeField] private TextMeshProUGUI DexNeed;

    [SerializeField] private TextMeshProUGUI Luk;
    [SerializeField] private TextMeshProUGUI LukNeed;

    [SerializeField] private TextMeshProUGUI Atk;
    [SerializeField] private TextMeshProUGUI Def;
    [SerializeField] private TextMeshProUGUI MAtk;
    [SerializeField] private TextMeshProUGUI MDef;
    [SerializeField] private TextMeshProUGUI Hit;
    [SerializeField] private TextMeshProUGUI Flee;
    [SerializeField] private TextMeshProUGUI Crit;
    [SerializeField] private TextMeshProUGUI Aspd;
    [SerializeField] private TextMeshProUGUI Points;
    [SerializeField] private TextMeshProUGUI Guild;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateData(ZC.STATUS stats) {
        Str.text = stats.str.ToString();
        StrNeed.text = stats.needStr.ToString();

        Agi.text = stats.agi.ToString();
        AgiNeed.text = stats.needAgi.ToString();

        Vit.text = stats.vit.ToString();
        VitNeed.text = stats.needVit.ToString();

        Int.text = stats.inte.ToString();
        IntNeed.text = stats.needInte.ToString();

        Dex.text = stats.dex.ToString();
        DexNeed.text = stats.needDex.ToString();

        Luk.text = stats.luk.ToString();
        LukNeed.text = stats.needLuk.ToString();

        Atk.text = $"{stats.atk} ~ {stats.atk2}";
        Def.text = $"{stats.def} ~ {stats.def2}";
        MAtk.text = $"{stats.matkMin} ~ {stats.matkMax}";
        MDef.text = $"{stats.mdef} ~ {stats.mdef2}";
        Hit.text = $"{stats.hit}";
        Flee.text = $"{stats.flee} ~ {stats.flee2}";
        Crit.text = $"{stats.crit}";
        Aspd.text = $"{(2000 - stats.aspd) / 10}";
        Points.text = $"{stats.stpoint}";
    }

    public void UpdateData(string value, EntityStatus? status) {
        if (status == null) return;

        switch (status) {
            case EntityStatus.SP_STR:
                Str.text = value;
                break;
            case EntityStatus.SP_AGI:
                Agi.text = value;
                break;
            case EntityStatus.SP_VIT:
                Vit.text = value;
                break;
            case EntityStatus.SP_INT:
                Int.text = value;
                break;
            case EntityStatus.SP_DEX:
                Dex.text = value;
                break;
            case EntityStatus.SP_LUK:
                Luk.text = value;
                break;
        }
    }
}