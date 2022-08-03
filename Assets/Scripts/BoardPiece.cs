using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardPiece : MonoBehaviour
{
    private Tile tile;
    private int Xpos;
    private int Ypos;

    public void Init(int x, int y , Tile tile)
    {
        this.Xpos = x;
        this.Ypos = y;
        this.tile = tile;
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
       
    }
    public Tile GetTile()
    {
        return this.tile;
    }
    public void SetPosition(int x, int y)
    {
        Xpos = x;
        Ypos = y;
    }
    public int GetX()
    {
        return Xpos;
    }
    public int GetY()
    {
        return Ypos;
    }
  
  

}
