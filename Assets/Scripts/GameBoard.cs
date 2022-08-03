using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    private Tilemap tilemap;

    public void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
    }
    public void Draw(BoardPiece[,] gameMatrix)
    {

        for(int i= 0; i < gameMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gameMatrix.GetLength(1); j++)
            {
                if(gameMatrix[i, j] != null)
                {
                    tilemap.SetTile(new Vector3Int(gameMatrix[i, j].GetY(), -gameMatrix[i, j].GetX(), 0), gameMatrix[i, j].GetTile());
                }
                else
                {

                    tilemap.SetTile(new Vector3Int(j, -i, 0), null);
                }
            }
        }
        
    }
    
}
