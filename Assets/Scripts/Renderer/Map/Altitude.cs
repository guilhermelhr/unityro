using System;
using UnityEngine;
/// <summary>
/// Altiture renderer
/// 
/// @author Guilherme Hernandez
/// Based on ROBrowser by Vincent Thibault (robrowser.com)
/// </summary>
public class Altitude {
    private static int MAX_INTERSECT_COUNT = 150;
    private GAT gat;

    public Altitude(BinaryReader stream) {
        gat = AltitudeLoader.Load(stream);

        //TODO init pathfinding
    }

    public Altitude(GAT gat) {
        this.gat = gat;

        //TODO init pathfinding
    }

    public long getHeight() {
        return gat.height;
    }

    public long getWidth() {
        return gat.width;
    }

    public long GetCellCount() {
        return gat.width * gat.height;
    }

    /// <summary>
    /// Get cell data
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>cell data</returns>
    public AltitudeLoader.Cell GetCell(double x, double y) {
        uint index = (uint) (Math.Floor(x) + Math.Floor(y) * gat.width);

        return gat.cells[index];
    }

    /// <summary>
    /// Return cell type
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>cell type</returns>
    public byte GetCellType(double x, double y) {
        return (byte) GetCell(x, y).type;
    }

    /// <summary>
    /// Return cell height
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns>cell height</returns>
    public double GetCellHeight(double x, double y) {
        if(gat.cells == null) {
            return 0;
        }

        /* DIFF robrowser adds 0.5 to each coordinate here */

        AltitudeLoader.Cell cell = GetCell(x, y);

        x = Math.Floor(x);
        y = Math.Floor(y);

        double x1 = cell.heights[0] + (cell.heights[1] - cell.heights[0]) * x;
        double x2 = cell.heights[2] + (cell.heights[3] - cell.heights[2]) * x;

        return -(x1 + (x2 - x1) * y);
    }

    /// <summary>
    /// Intersect cell
    /// </summary>
    /// <param name="output">vector</param>
    /// <returns>sucess</returns>
    public bool Intersect(Vector2 output) {
        var _from = new Vector3();
        var _to = new Vector4();
        Vector3 _unit;
        Matrix4x4 _matrix;

        //extract camera position
        _matrix = Matrix4x4.Inverse(GL.modelview);
        _from[0] = _matrix[12];
        _from[1] = _matrix[13];
        _from[2] = _matrix[14];


        //  set two vectors with opposing z values
        Vector2 mouse = Conversions.GetMouseTopLeft();
        _to[0] = (mouse.x / Screen.width) * 2 - 1;
        _to[1] = -(mouse.y / Screen.height) * 2 + 1;
        _to[2] = 1.0f;
        _to[3] = 1.0f;


        // Unproject
        _matrix = UnityEngine.Camera.current.projectionMatrix * GL.modelview;
        _matrix = _matrix.inverse;
        _to = Conversions.TransformMat4(_to, _matrix);

        _to[0] /= _to[3];
        _to[1] /= _to[3];
        _to[2] /= _to[3];

        // Extract direction
        _unit = new Vector3(_to[0], _to[1], _to[2]) - _from;
        _unit.Normalize();

        // Search
        for(int i = 0; i < MAX_INTERSECT_COUNT; i++) {
            _from[0] += _unit[0];
            _from[1] += _unit[1];
            _from[2] += _unit[2];

            if(Math.Abs(GetCellHeight(_from[0], _from[2]) + _from[1]) < 0.5) {
                output[0] = _from[0];
                output[1] = _from[2];
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Generate a plane stick to the ground
    /// Used for effects
    /// </summary>
    /// <param name="pos_x">x position</param>
    /// <param name="pos_y">y position</param>
    /// <param name="size">plane size</param>
    /// <returns>plane (two triangles)</returns>
    public float[] GeneratePlane(double dpos_x, double dpos_y, int size) {
        if(gat.cells == null) {
            return null;
        }

        //DIFF robrowser does a switch here to "avoid memory allocation" that seems very redundant to me
        float[] buffer = new float[size * size * 30];
        int middle = (int) Math.Floor(size / 2f);
        int pos_x = (int) Math.Floor(dpos_x);
        int pos_y = (int) Math.Floor(dpos_y);

        int i = 0;
        for(int x = -middle; x <= middle; x++) {
            for(int y = -middle; y <= middle; y++, i+=30) {
                int index = ((pos_x + x) + (pos_y + y) * (int) gat.width);

                // Triangle 1
                buffer[i + 0] = pos_x + x + 0;
                buffer[i + 1] = gat.cells[index].heights[0];
                buffer[i + 2] = pos_y + y + 0;
                buffer[i + 3] = (x + 0 + middle) / size;
                buffer[i + 4] = (y + 0 + middle) / size;

                buffer[i + 5] = pos_x + x + 1;
                buffer[i + 6] = gat.cells[index].heights[1];
                buffer[i + 7] = pos_y + y + 0;
                buffer[i + 8] = (x + 1 + middle) / size;
                buffer[i + 9] = (y + 0 + middle) / size;

                buffer[i + 10] = pos_x + x + 1;
                buffer[i + 11] = gat.cells[index].heights[3];
                buffer[i + 12] = pos_y + y + 1;
                buffer[i + 13] = (x + 1 + middle) / size;
                buffer[i + 14] = (y + 1 + middle) / size;

                // Triangle 2
                buffer[i + 15] = pos_x + x + 1;
                buffer[i + 16] = gat.cells[index].heights[3];
                buffer[i + 17] = pos_y + y + 1;
                buffer[i + 18] = (x + 1 + middle) / size;
                buffer[i + 19] = (y + 1 + middle) / size;

                buffer[i + 20] = pos_x + x + 0;
                buffer[i + 21] = gat.cells[index].heights[2];
                buffer[i + 22] = pos_y + y + 1;
                buffer[i + 23] = (x + 0 + middle) / size;
                buffer[i + 24] = (y + 1 + middle) / size;

                buffer[i + 25] = pos_x + x + 0;
                buffer[i + 26] = gat.cells[index].heights[0];
                buffer[i + 27] = pos_y + y + 0;
                buffer[i + 28] = (x + 0 + middle) / size;
                buffer[i + 29] = (y + 0 + middle) / size;
            }
        }

        return buffer;
    }
}
