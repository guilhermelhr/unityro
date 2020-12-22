using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class CustomScriptLoader : ScriptLoaderBase {
    public override object LoadFile(string file, Table globalContext) {
        return new StreamReader(file, System.Text.Encoding.GetEncoding(1252)).ReadToEnd();
    }

    public override bool ScriptFileExists(string name) {
        return File.Exists(name);
    }
}
