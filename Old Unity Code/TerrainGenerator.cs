using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
[RequireComponent(typeof(Tilemap))]
public class TerrainGenerator : MonoBehaviour
{

    private Tilemap tm;
    public WeightedTile[] Tiles;
    private System.Random rnd = new System.Random();
    void Start()
    {
        tm = GetComponent<Tilemap>();
        GenerateMap(new Vector3Int(-50, -50, 0), new Vector3Int(50, 50, 0));
    }
    public void GenerateMap(Vector3Int Min, Vector3Int Max)
    {
        AddBioms(Min, Max);
        while (!ContainsTerain(Min, Max))
        {
            AddBioms(Min, Max);
        }
        while (ContainsNull(Min, Max))
        {
            for (int x = Min.x; x < Max.x; x++)
            {
                for (int y = Min.y; y < Max.y; y++)
                {
                    SetNeibors(new Vector3Int(x, y, 0), Min, Max);
                }
            }
        }
    }
    private void SetNeibors(Vector3Int pos, Vector3Int Min, Vector3Int Max)
    {
        if (tm.GetTile(pos) != null)
        {
            WeightedTile t = (WeightedTile)tm.GetTile(pos);
            Vector3Int p = pos + new Vector3Int(0, 1, 0);
            if (Chance(t.SpreadChance))
            {
                if (p.x < Max.x && p.y < Max.y && p.x >= Min.x && p.y >= Min.y)
                {
                    if (tm.GetTile(p) == null)
                    {
                        tm.SetTile(p, t);
                    }
                }
            }
            p = pos + new Vector3Int(0, -1, 0);
            if (Chance(t.SpreadChance))
            {
                if (p.x < Max.x && p.y < Max.y && p.x >= Min.x && p.y >= Min.y)
                {
                    if (tm.GetTile(p) == null)
                    {
                        tm.SetTile(p, t);
                    }
                }
            }
            p = pos + new Vector3Int(1, 0, 0);
            if (Chance(t.SpreadChance))
            {
                if (p.x < Max.x && p.y < Max.y && p.x >= Min.x && p.y >= Min.y)
                {
                    if (tm.GetTile(p) == null)
                    {
                        tm.SetTile(p, t);
                    }
                }
            }
            p = pos + new Vector3Int(-1, 0, 0);
            if (Chance(t.SpreadChance))
            {
                if (p.x < Max.x && p.y < Max.y && p.x >= Min.x && p.y >= Min.y)
                {
                    if (tm.GetTile(p) == null)
                    {
                        tm.SetTile(p, t);
                    }
                }
            }
        }
    }
    private bool ContainsTerain(Vector3Int Min, Vector3Int Max)
    {
        for (int x = Min.x; x < Max.x; x++)
        {
            for (int y = Min.y; y < Max.y; y++)
            {
                if (tm.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool ContainsNull(Vector3Int Min, Vector3Int Max)
    {
        for (int x = Min.x; x < Max.x; x++)
        {
            for (int y = Min.y; y < Max.y; y++)
            {
                if (tm.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool Chance(int Chance)
    {
        if (rnd.Next(0, 99) < Chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AddBioms(Vector3Int Min, Vector3Int Max)
    {
        for (int x = Min.x; x < Max.x; x++)
        {
            for (int y = Min.y; y < Max.y; y++)
            {
                foreach (WeightedTile t in Tiles)
                {
                    if (Chance(t.BiomialChance))
                    {
                        tm.SetTile(new Vector3Int(x, y, 0), t);
                    }
                }
            }
        }
    }
}
