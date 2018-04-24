using System.IO;
/// <summary>
/// .gat file representation
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class GAT {
    public static string Header = "GRAT";

    public long width;
    public long height;
    public AltitudeLoader.Cell[] cells;
    public string version;

    public GAT(uint width, uint height, AltitudeLoader.Cell[] cells, string version) {
        this.width = width;
        this.height = height;
        this.cells = cells;
        this.version = version;
    }

    public override string ToString() {
        return "GAT v" + version + "(" + width + "x" + height + ")";
    }
}
