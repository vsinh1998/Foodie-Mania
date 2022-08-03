using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int position { get; private set; }
    public TetrominoData data { get; private set; }

    public Vector3Int[] cells { get; private set; }

    public float stepDelay = 1.0f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;
    public void Initialize(Board board, Vector3Int position,TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.stepTime = Time.time + stepDelay;
        this.lockTime = 0;
        if (this.cells == null)
        { 
            cells = new Vector3Int[data.cells.Length];
        }
    
        for (int i = 0; i < data.cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
       
    }
    public void Update()
    {
        this.board.Clear(this);

        lockTime += Time.deltaTime;

       
        if(Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SoftDrop();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        if(Time.time > stepTime)
        {
            Step();
        }
        this.board.Set(this);
    }
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if(this.lockTime > this.lockDelay)
        {
            Lock();
        }
    }
    private void Lock()
    {
        this.board.Set(this);
        this.board.SpawnPiece();
    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        bool valid = this.board.IsValidPosition(this,newPosition);
        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0;
        }
        return valid;

    }
    private void SoftDrop()
    {
        Move(Vector2Int.down);
    }
    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }
}
