using System.Collections.Generic;
using System;
using UnityEngine;

public class MDRPG_SaveFile
{
    public string Name;
    public string Asset_Pack;
    public DateTime Last_Played;
    public DateTime Time_Played;
    public Vector2 Player_Position;
    public string Player_Demention;
    public List<string> Player_Inventory;
    public int Player_Health;
    public List<Demention_Data> Demention_Data;
}
public struct TileData
{
    public string Tile_Name;
    public Vector2Int Tile_Position;
    TileData(string TileName, Vector2Int TilePos)
    {
        this.Tile_Name = TileName;
        this.Tile_Position = TilePos;
    }
}
public class Demention_Data
{
    public string Name;
    public List<TileData> Object_Data = new List<TileData>();
    public List<TileData> Biome_Data = new List<TileData>();
}