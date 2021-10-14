using MoonSharp.Interpreter;
using ROIO;
using System.IO;
using System.Text;

public class LuaInterface {

    public static Script Environment { get; private set; } = new Script();

    public LuaInterface(Configuration configs) {
        LoadSkillInfoZ();
        LoadJobInfo();
        LoadAccessoryInfo();

        ItemTable.LoadItemDb(configs);
        SkillTable.LoadSkillData();
    }

    public static Table GetTable(string name) {
        return Environment.Globals[name] as Table;
    }

    private void LoadSkillInfoZ() {
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/jobinheritlist.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillid.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilldescript.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfolist.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilltreeview.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfo_f.lub") as MemoryStream);
    }

    private void LoadJobInfo() {
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobidentity.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/npcidentity.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobname.lub") as MemoryStream);
        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/jobname.lub") as MemoryStream);
        //environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender_f.lub"));

        /**
         * Hack for Kagerou and Oboro
         * It seems like Gravity doesnt like to have a common ground for their scripts
         */
        var JTtbl = Environment.Globals["JTtbl"] as Table;
        Environment.Globals["pcJobTbl2"] = JTtbl;

        Environment.DoStream(FileManager.ReadSync("data/luafiles514/lua files/datainfo/pcjobnamegender.lub") as MemoryStream);
    }

    private void LoadAccessoryInfo() {
        Environment.DoString(FileManager.ReadSync("data/lua files/datainfo/accessoryid.lub", Encoding.GetEncoding(1252)).ReadToEnd());
        Environment.DoString(FileManager.ReadSync("data/lua files/datainfo/accname.lub", Encoding.GetEncoding(1252)).ReadToEnd());
    }
}
