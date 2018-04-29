
using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundLoader {
    public static GND Load(BinaryReader data) {
        string header = data.ReadBinaryString(4);

        if(!string.Equals(header, GND.Header)) {
            throw new Exception("GroundLoader.Load: Header (" + header + ") is not \"GRGN\"");
        }

        string version = Convert.ToString(data.ReadUByte());
        string subversion = Convert.ToString(data.ReadUByte());
        version += "." + subversion;

        GND gnd = new GND(version);
        gnd.width = data.ReadULong();
        gnd.height = data.ReadULong();
        gnd.zoom = data.ReadFloat();

        ParseTextures(gnd, data);
        ParseLightmaps(gnd, data);

        gnd.tiles = ParseTiles(gnd, data);
        gnd.surfaces = ParseSurfaces(gnd, data);

        return gnd;
    }

    private static void ParseTextures(GND gnd, BinaryReader data) {
        uint textureCount = data.ReadULong();
        uint texturePathLength = data.ReadULong();
        int[] lookupList = new int[textureCount];
        List<string> textures = new List<string>();

        for(int i = 0; i < textureCount; i++) {
            string texture = data.ReadBinaryString(texturePathLength);
            int pos = textures.IndexOf(texture);

            if(pos == -1) {
                textures.Add(texture);
                pos = textures.Count - 1;
            }

            lookupList[i] = pos;
        }

        gnd.textures = textures.ToArray();
        gnd.textureLookupList = lookupList;
    }

    private static void ParseLightmaps(GND gnd, BinaryReader data) {
        uint count = data.ReadULong();
        int perCellX = data.ReadLong();
        int perCellY = data.ReadLong();
        int sizeCell = data.ReadLong();
        int perCell = perCellX * perCellY * sizeCell;

        var lightmap = gnd.lightmap = new GND.Lightmap();
        lightmap.count = count;
        lightmap.perCell = perCell;
        lightmap.data = new byte[count * perCell * 4];
        data.Read(lightmap.data, 0, lightmap.data.Length);
    }

    private static GND.Tile[] ParseTiles(GND gnd, BinaryReader data) {
        uint count = data.ReadULong();
        GND.Tile[] tiles = new GND.Tile[count];

        var ATLAS_COLS = Math.Round(Math.Sqrt(gnd.textures.Length));
        var ATLAS_ROWS = Math.Ceiling(Math.Sqrt(gnd.textures.Length));
        var ATLAS_WIDTH = Math.Pow(2, Math.Ceiling(Math.Log(ATLAS_COLS * 258) / Math.Log(2)));
        var ATLAS_HEIGHT = Math.Pow(2, Math.Ceiling(Math.Log(ATLAS_ROWS * 258) / Math.Log(2)));
        var ATLAS_FACTOR_U = (ATLAS_COLS * 258) / ATLAS_WIDTH;
        var ATLAS_FACTOR_V = (ATLAS_ROWS * 258) / ATLAS_HEIGHT;
        var ATLAS_PX_U = 1 / 258f;
        var ATLAS_PX_V = 1 / 258f;

        for(int i = 0; i < count; i++) {
            var tile = tiles[i] = new GND.Tile();
            tile.textureStart = new Vector4(data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
            tile.textureEnd = new Vector4(data.ReadFloat(), data.ReadFloat(), data.ReadFloat(), data.ReadFloat());
            tile.texture = data.ReadUShort();
            tile.light = data.ReadUShort();
            tile.color = new byte[] { data.ReadUByte(), data.ReadUByte(), data.ReadUByte(), data.ReadUByte()};
            tile.texture = (ushort) gnd.textureLookupList[tile.texture];

            var start = tile.texture % ATLAS_COLS;
            var end = Math.Floor(tile.texture / ATLAS_COLS);

            for(int j = 0; j < 4; j++) {
                tile.textureStart[j] = (float) ((start + tile.textureStart[j] * (1 - ATLAS_PX_U * 2) + ATLAS_PX_U) * ATLAS_FACTOR_U / ATLAS_COLS);
                tile.textureEnd[j] = (float) ((end + tile.textureEnd[j] * (1 - ATLAS_PX_V * 2) + ATLAS_PX_V) * ATLAS_FACTOR_V / ATLAS_ROWS);
            }
        }

        return tiles;
    }

    private static GND.Surface[] ParseSurfaces(GND gnd, BinaryReader data) {
        var count = gnd.width * gnd.height;
        GND.Surface[] surfaces = new GND.Surface[count];

        for(int i = 0; i < count; i++) {
            var surface = surfaces[i] = new GND.Surface();
            surface.height = new Vector4(data.ReadFloat() / 5, data.ReadFloat() / 5, data.ReadFloat() / 5, data.ReadFloat() / 5);
            surface.tileUp = data.ReadLong();
            surface.tileFront = data.ReadLong();
            surface.tileRight = data.ReadLong();
        }

        return surfaces;
    }

    private static Vector3[][] GetSmoothNormal(GND gnd) {
        Vector3 a = new Vector3();
        Vector3 b = new Vector3();
        Vector3 c = new Vector3();
        Vector3 d = new Vector3();
        var count = gnd.width * gnd.height;
        var tmp = new Vector3?[count];
        var normals = new Vector3[count][];
        var emptyVec = new Vector3();

        //calculate normal for each cell
        for(int y = 0; y < gnd.height; y++) {
            for(int x = 0; x < gnd.width; x++) {
                var cell = gnd.surfaces[x + y * gnd.width];

                if(cell.tileUp > -1) {
                    a[0] = (x + 0) * 2; a[1] = cell.height[0]; a[2] = (y + 0) * 2;
                    b[0] = (x + 1) * 2; b[1] = cell.height[1]; b[2] = (y + 0) * 2;
                    c[0] = (x + 1) * 2; c[1] = cell.height[3]; c[2] = (y + 1) * 2;
                    d[0] = (x + 0) * 2; d[1] = cell.height[2]; d[2] = (y + 1) * 2;

                    tmp[x + y * gnd.width] = Conversions.CalcNormal(a, b, c, d);
                }
            }
        }

        //smooth normals
        for(int y = 0; y < gnd.height; y++) {
            for(int x = 0; x < gnd.width; x++) {
                var n = normals[x + y * gnd.width] = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

                for(byte i = 0; i < 4; i++) {
                    int b1 = i & 1;
                    int b2 = (i & 2) >> 1;
                    int xor = b1 ^ b2;
                    n[i] += Conversions.safeArrayAccess(tmp, (x) + (y) * gnd.width, emptyVec) ?? emptyVec;
                    n[i] += Conversions.safeArrayAccess(tmp, (x + 1 * -(xor)) + (y) * gnd.width, emptyVec) ?? emptyVec;
                    n[i] += Conversions.safeArrayAccess(tmp, (x + 1 * -(xor)) + (y + 1 * -(b2)) * gnd.width, emptyVec) ?? emptyVec;
                    n[i] += Conversions.safeArrayAccess(tmp, (x) + (y + 1 * -(b2)) * gnd.width, emptyVec) ?? emptyVec;
                    n[i].Normalize();
                }
            }
        }

        return normals;
    }

    private static byte[] CreateLightmapImage(GND gnd) {
        var lightmap = gnd.lightmap;
        var count = lightmap.count;
        var data = lightmap.data;
        var perCell = lightmap.perCell;

        var width = Math.Round(Math.Sqrt(count));
        var height = Math.Ceiling(Math.Sqrt(count));
        var _width = Math.Pow(2, Math.Ceiling(Math.Log(width * 8) / Math.Log(2)));
        var _height = Math.Pow(2, Math.Ceiling(Math.Log(height * 8) / Math.Log(2)));

        byte[] image = new byte[(int) (_width * _height * 4)];

        for(int i = 0; i < count; i++) {
            var pos = i * 4 * perCell;
            var x = (i % width) * 8;
            var y = Conversions.SafeDivide(i, width) * 8;

            for(int _x = 0; _x < 8; _x++) {
                for(int _y = 0; _y < 8; _y++) {
                    int idx = (int) ((x + _x) + (y + _y) * _width) * 4;
                    //TODO test removing >> 4 << 4
                    image[idx + 0] = (byte) (data[pos + perCell + (_x + _y * 8) * 3 + 0] >> 4 << 4); // Posterisation
                    image[idx + 1] = (byte) (data[pos + perCell + (_x + _y * 8) * 3 + 1] >> 4 << 4); // Posterisation
                    image[idx + 2] = (byte) (data[pos + perCell + (_x + _y * 8) * 3 + 2] >> 4 << 4); // Posterisation
                    image[idx + 3] = data[pos + (_x + _y * 8)];
				}
            }
        }

        return image;
    }

    private static byte[] CreateTilesColorImage(GND gnd) {
        byte[] data = new byte[gnd.width * gnd.height * 4];

        for(int y = 0; y < gnd.height; ++y) {
            for(int x = 0; x < gnd.width; ++x) {
                var cell = gnd.surfaces[x + y * gnd.width];

                // Check tile up
                if(cell.tileUp > -1) {
                    var color = gnd.tiles[cell.tileUp].color;
                    color.CopyTo(data, (x + y * gnd.width) * 4);
                }
            }
        }

        return data;
    }

    private static byte[] CreateShadowmapData(GND gnd) {
        var _out = new byte[(gnd.width * 8) * (gnd.height * 8)];
        var data = gnd.lightmap.data;

        for(int y = 0; y < gnd.height; y++) {
            for(int x = 0; x < gnd.width; x++) {

                var cell = gnd.surfaces[x + y * gnd.width];

                if(cell.tileUp > -1) {
                    var index = gnd.tiles[cell.tileUp].light * 4 * gnd.lightmap.perCell;

                    for(int i = 0; i < 8; i++) {
                        for(int j = 0; j < 8; j++) {
							_out[(x * 8 + i) + (y* 8 + j) * (gnd.width* 8)] = data[index + i + j * 8];
						}
					}
				}

				// If no ground, shadow should be 1.0
				else {
					for(int i = 0; i < 8; i++) {
						for(int j = 0; j < 8; j++) {
							_out[(x * 8 + i) + (y * 8 + j) * (gnd.width * 8)] = 255;
						}
					}
				}
			}
		}


        return _out;
    }

    public static GND.Mesh Compile(GND gnd, float WATER_LEVEL, float WATER_HEIGHT) {
        var normals = GetSmoothNormal(gnd);

        var lightmapData = CreateLightmapImage(gnd);
        var l_count_w = Math.Round(Math.Sqrt(lightmapData.Length));
        var l_count_h = Math.Ceiling(Math.Sqrt(lightmapData.Length));
        var l_width = Math.Pow(2, Math.Ceiling(Math.Log(l_count_w * 8) / Math.Log(2)));
        var l_height = Math.Pow(2, Math.Ceiling(Math.Log(l_count_h * 8) / Math.Log(2)));

        var meshData = new List<float>();
        var waterMeshData = new List<float>();

        for(int y = 0; y < gnd.height; y++) {
            for(int x = 0; x < gnd.width; x++) {

                var cellA = gnd.surfaces[x + y * gnd.width];
                var h_a = cellA.height;

                // Check tile up
                if(cellA.tileUp > -1) {
                    var tile = gnd.tiles[cellA.tileUp];

                    // Check if has texture
                    var n = normals[x + y * gnd.width];

                    float lu1 = (float) ((((tile.light % l_count_w) + 0.125) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lu2 = (float) ((((tile.light % l_count_w) + 0.875) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lv1 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.125) / l_count_h) * ((l_count_h * 8) / l_height));
                    float lv2 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.875) / l_count_h) * ((l_count_h * 8) / l_height));

                    meshData.AddRange(new float[] {
                        (x + 0) * 2, h_a[0], (y + 0) * 2, n[0][0], n[0][1], n[0][1], tile.textureStart[0], tile.textureEnd[0], lu1, lv1, (x + 0.5f) / gnd.width, (y + 0.5f) / gnd.height,
                        (x + 1) * 2, h_a[1], (y + 0) * 2, n[1][0], n[1][1], n[1][1], tile.textureStart[1], tile.textureEnd[1], lu2, lv1, (x + 1.5f) / gnd.width, (y + 0.5f) / gnd.height,
                        (x + 1) * 2, h_a[3], (y + 1) * 2, n[2][0], n[2][1], n[2][1], tile.textureStart[3], tile.textureEnd[3], lu2, lv2, (x + 1.5f) / gnd.width, (y + 1.5f) / gnd.height,
                        //(x + 1) * 2, h_a[3], (y + 1) * 2, n[2][0], n[2][1], n[2][1], tile.textureStart[3], tile.textureEnd[3], lu2, lv2, (x + 1.5f) / gnd.width, (y + 1.5f) / gnd.height,
                        (x + 0) * 2, h_a[2], (y + 1) * 2, n[3][0], n[3][1], n[3][1], tile.textureStart[2], tile.textureEnd[2], lu1, lv2, (x + 0.5f) / gnd.width, (y + 1.5f) / gnd.height,
                        //(x + 0) * 2, h_a[0], (y + 0) * 2, n[0][0], n[0][1], n[0][1], tile.textureStart[0], tile.textureEnd[0], lu1, lv1, (x + 0.5f) / gnd.width, (y + 0.5f) / gnd.height
                    });

                    // Add water only if it's upper than the ground.
                    if(h_a[0] > WATER_LEVEL - WATER_HEIGHT ||
                        h_a[1] > WATER_LEVEL - WATER_HEIGHT ||
                        h_a[2] > WATER_LEVEL - WATER_HEIGHT ||
                        h_a[3] > WATER_LEVEL - WATER_HEIGHT) {

                        float o = 5f;
                        var texx = ((x + 1) % o) / o;
                        if(texx == 0) {
                            texx = 1;
                        }
                        var texy = ((y + 1) % o) / o;
                        if(texy == 0) {
                            texy = 1;
                        }
                        waterMeshData.AddRange(new float[] {
                            //        vec3 pos            |            vec2 texcoords
                            (x + 0) * 2, WATER_LEVEL, (y + 0) * 2, (((x + 0) % o) / o), (((y + 0) % o) / o),
                            (x + 1) * 2, WATER_LEVEL, (y + 0) * 2, texx, (((y + 0) % o) / o),
                            (x + 1) * 2, WATER_LEVEL, (y + 1) * 2, texx, texy,
                            //(x + 1) * 2, WATER_LEVEL, (y + 1) * 2, texx, texy,
                            (x + 0) * 2, WATER_LEVEL, (y + 1) * 2, (((x + 0) % o) / o), texy,
                            //(x + 0) * 2, WATER_LEVEL, (y + 0) * 2, ((x + 0) % 5 / 5), ((y + 0) % 5 / 5)
                        });
                    }
                }

                // Check tile front
                if((cellA.tileFront > -1) && (y + 1 < gnd.height)) {
                    var tile = gnd.tiles[cellA.tileFront];

                    var cellB = gnd.surfaces[x + (y + 1) * gnd.width];
                    var h_b = cellB.height;

                    float lu1 = (float) ((((tile.light % l_count_w) + 0.125) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lu2 = (float) ((((tile.light % l_count_w) + 0.875) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lv1 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.125) / l_count_h) * ((l_count_h * 8) / l_height));
                    float lv2 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.875) / l_count_h) * ((l_count_h * 8) / l_height));

                    meshData.AddRange(new float[] {
                        //      vec3 pos           |  vec3 normals     |    vec2 texcoords      |   vec2 lightcoord  |   vec2 tileCoords
                        (x + 0) * 2, h_b[0], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[2], tile.textureEnd[2], lu1, lv2, 0, 0,
                        (x + 0) * 2, h_a[2], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[0], tile.textureEnd[0], lu1, lv1, 0, 0,
                        (x + 1) * 2, h_a[3], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[1], tile.textureEnd[1], lu2, lv1, 0, 0,
                        (x + 1) * 2, h_b[1], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[3], tile.textureEnd[3], lu2, lv2, 0, 0,
                        //(x + 0) * 2, h_b[0], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[2], tile.textureEnd[2], lu1, lv2, 0, 0,
                        //(x + 1) * 2, h_a[3], (y + 1) * 2, 0.0f, 0.0f, 1.0f, tile.textureStart[1], tile.textureEnd[1], lu2, lv1, 0, 0,
                        
                    });
                }

                // Check tile right
                if((cellA.tileRight > -1) && (x + 1 < gnd.width)) {
                    var tile = gnd.tiles[cellA.tileRight];

                    var cellB = gnd.surfaces[(x + 1) + y * gnd.width];
                    var h_b = cellB.height;
                    
                    float lu1 = (float) ((((tile.light % l_count_w) + 0.125) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lu2 = (float) ((((tile.light % l_count_w) + 0.875) / l_count_w) * ((l_count_w * 8) / l_width));
                    float lv1 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.125) / l_count_h) * ((l_count_h * 8) / l_height));
                    float lv2 = (float) ((((Conversions.SafeDivide(tile.light, l_count_w)) + 0.875) / l_count_h) * ((l_count_h * 8) / l_height));

                    meshData.AddRange(new float[] {
                        //      vec3 pos           |  vec3 normals    |    vec2 texcoords      |   vec2 lightcoord   |    vec2 tileCoords
                        (x + 1) * 2, h_a[1], (y + 0) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[1], tile.textureEnd[1], lu2, lv1, 0, 0,
                        (x + 1) * 2, h_a[3], (y + 1) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[0], tile.textureEnd[0], lu1, lv1, 0, 0,
                        (x + 1) * 2, h_b[0], (y + 0) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[3], tile.textureEnd[3], lu2, lv2, 0, 0,
                        //(x + 1) * 2, h_b[0], (y + 0) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[3], tile.textureEnd[3], lu2, lv2, 0, 0,
                        (x + 1) * 2, h_b[2], (y + 1) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[2], tile.textureEnd[2], lu1, lv2, 0, 0,
                        //(x + 1) * 2, h_a[3], (y + 1) * 2, 1.0f, 0.0f, 0.0f, tile.textureStart[0], tile.textureEnd[0], lu1, lv1, 0, 0
                    });
                }
            }
        }

        var mesh = new GND.Mesh();
        
        mesh.width = gnd.width;
        mesh.height = gnd.height;
        mesh.textures = gnd.textures;

        mesh.lightmap = lightmapData;
        mesh.tileColor = CreateTilesColorImage(gnd);
        mesh.shadowMap = CreateShadowmapData(gnd);

        mesh.mesh = meshData.ToArray();
        mesh.meshVertCount = meshData.Count / 12;

        mesh.waterMesh = waterMeshData.ToArray();
        mesh.waterVertCount = waterMeshData.Count / 5;

        return mesh;
    }
}
