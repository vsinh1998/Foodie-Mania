using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    public   List<Tile> tiles = new List<Tile>();
    public  Tile GetTile(string name)
    {
        return tiles.Find(x => x.name == name);
    }
}
