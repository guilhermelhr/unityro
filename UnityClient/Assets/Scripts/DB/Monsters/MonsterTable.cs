using MoonSharp.Interpreter;
using System.Collections.Generic;

public static class JobItentityTable {

    public static Dictionary<int, string> Table;

    public static void LoadTable() {
        Table = new Dictionary<int, string>();
        var table = LuaInterface.GetTable("JobNameTable");
        foreach (var key in table.Keys) {
            var value = table[key].ToString();
            Table[int.Parse(key.ToString())] = value;
        }
    }
}
