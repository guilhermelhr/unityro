﻿using MoonSharp.Interpreter;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LuaInterface {

    public static Script Environment { get; private set; } = new Script();

    public LuaInterface() {
        InitTables();
    }

    public static Table GetTable(string name) {
        return Environment.Globals[name] as Table;
    }

    private async void InitTables() {
        await LoadSkillInfoZ();
        await LoadJobInfo();
        await LoadAccessoryInfo();

        await ItemTable.LoadItemDb();
        SkillTable.LoadSkillData();
    }

    private async Task LoadSkillInfoZ() {
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/jobinheritlist.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/skillid.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/skilldescript.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/skillinfolist.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/skillinfo_f.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/skillinfoz/skilltreeview.lub.txt"));
    }

    private async Task LoadJobInfo() {
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/jobidentity.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/npcidentity.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/jobname.lub.txt"));
        //environment.DoStream(Addressables.LoadAssetAsync<TextAsset>("data/luafiles514/lua files/datainfo/pcjobnamegender_f.lub"));

        /**
         * Hack for Kagerou and Oboro
         * It seems like Gravity doesnt like to have a common ground for their scripts
         */
        var JTtbl = Environment.Globals["JTtbl"] as Table;
        Environment.Globals["pcJobTbl2"] = JTtbl;

        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/pcjobnamegender.lub.txt"));
    }

    private async Task LoadAccessoryInfo() {
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/accessoryid.lub.txt"));
        Environment.DoString(await LoadTable("lua/data/luafiles514/lua files/datainfo/accname.lub.txt"));
    }

    private async Task<string> LoadTable(string key) {
        return (await Addressables.LoadAssetAsync<TextAsset>(key).Task).text;
    }
}
