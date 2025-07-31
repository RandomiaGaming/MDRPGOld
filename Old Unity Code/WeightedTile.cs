using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

[Serializable]
[CreateAssetMenu(fileName = "New Weighted Tile", menuName = "Tiles/Weighted Tile")]
public class WeightedTile : Tile {
    [Range(0, 100)]
    public int SpreadChance = 45;
    [Range(0, 100)]
    public int BiomialChance = 10;
}
