using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominos;
    public Piece activePiece { get; private set; }
    public Tilemap tilemap {  get; private set; }
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2,-this.boardSize.y/2);
            return new RectInt(position,this.boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponent<Piece>();
        for (int i = 0; i < tetrominos.Length; i++)
        {
            tetrominos[i].Initialize();
        }
    }
   
    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random =Random.Range(0,tetrominos.Length);
        TetrominoData data = tetrominos[random];
        this.activePiece.Initialize(this, spawnPosition, data);
        Set(activePiece);
    }
    public void Set(Piece piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tileposition = piece.cells[i] + piece.position;
            tilemap.SetTile(tileposition, piece.data.tile);
        }
    }
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tileposition = piece.cells[i] + piece.position;
            tilemap.SetTile(tileposition, null);
        }
    }
    public bool IsValidPosition(Piece piece,  Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for(int i=0; i < piece.cells.Length; i++)
        {
            Vector3Int tileposition = piece.cells[i] + position;

            if (this.tilemap.HasTile(tileposition))
            {
                return false;
            }
            if(!bounds.Contains((Vector2Int)tileposition))
            {
                return false;
            }
        }
        return true;
    }

}
