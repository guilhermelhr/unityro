using MoonSharp.Interpreter;

public class LuaInterface {

    public static Script environment { get; private set; } = new Script();

    public LuaInterface() {
        LoadSkillInfoZ();
        LoadJobInfo();

        ItemTable.LoadItemDb();
        SkillTable.LoadSkillData();
    }

    public static Table GetTable(string name) {
        return environment.Globals[name] as Table;
    }

    private void LoadSkillInfoZ() {
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/jobinheritlist.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillid.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilldescript.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfolist.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilltreeview.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfo_f.lub"));
    }

    private void LoadJobInfo() {
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobidentity.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/npcidentity.lub"));
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobname.lub"));
        //environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender_f.lub"));

        /**
         * Hack for Kagerou and Oboro
         * It seems like Gravity doesnt like to have a common ground for their scripts
         */
        var JTtbl = environment.Globals["JTtbl"] as Table;
        environment.Globals["pcJobTbl2"] = JTtbl;

        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender.lub"));
    }
}
