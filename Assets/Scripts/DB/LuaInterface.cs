using MoonSharp.Interpreter;
using ROIO;
using System.IO;

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
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/jobinheritlist.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillid.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilldescript.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfolist.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilltreeview.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfo_f.lub") as MemoryStream);
    }

    private void LoadJobInfo() {
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobidentity.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/npcidentity.lub") as MemoryStream);
        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobname.lub") as MemoryStream);
        //environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender_f.lub"));

        /**
         * Hack for Kagerou and Oboro
         * It seems like Gravity doesnt like to have a common ground for their scripts
         */
        var JTtbl = environment.Globals["JTtbl"] as Table;
        environment.Globals["pcJobTbl2"] = JTtbl;

        environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender.lub") as MemoryStream);
    }
}
