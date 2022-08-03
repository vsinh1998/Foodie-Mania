using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    
    private List<int> orders = new List<int>();
    public int maxOrders;
    public int ordernumber = -1;
    private List<Recipe> activeOrders = new List<Recipe>();
    public int maxActiveOrders;
    private RecipeList recipes;
    private List<string> activeOrderIngridients = new List<string>();

    public TileManager TileManager;
    public GameBoard gameboard;
    public Vector3 spawnPosition;
    private BoardPiece activeBoardPiece;
    private BoardPiece[,] gamematrix = new BoardPiece[ROW, COLOUMN];

    private const int COLOUMN = 20;
    private const int ROW = 10;
    private float stepDelay = 0.3f;
    private float stepTimer = 0.0f;

    private Utility utility = new Utility();

    private static GameManager instance = null;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }
    private void Awake()
    {

        recipes = utility.LoadJsonData("Resources/Recipes.json");
        NewGame();
    }
    private void Start()
    {
        SpawnBoardPiece();
    }
    private void SpawnBoardPiece()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<BoardPiece>();
        obj.name = activeOrderIngridients[Random.Range(0, activeOrderIngridients.Count)];
        
        BoardPiece ip = obj.GetComponent<BoardPiece>();
        ip.Init((int)spawnPosition.x, (int)spawnPosition.y, TileManager.GetTile(obj.name));

        activeBoardPiece = ip;

        SetPosition(activeBoardPiece, activeBoardPiece.GetX(), activeBoardPiece.GetY());

      
    }

    private void Update()
    {
        stepTimer += Time.deltaTime;
        if (stepTimer > stepDelay)
        {
            stepTimer = 0;
            if (!Fall(activeBoardPiece))
            {
                CheckRecipe(activeBoardPiece);
                SpawnBoardPiece();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(activeBoardPiece, 0, -1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(activeBoardPiece, 0, 1);
        }
        gameboard.Draw(gamematrix);
    }

    private bool Fall(BoardPiece piece)
    {

        if (Move(piece, 1, 0))
        {
            return true;
        }
        return false;
    }
    private bool Move(BoardPiece piece, int xoffset, int yoffset) // x and y are in matrix coordinates
    {
        if (IsValidPosition(piece.GetX() + xoffset, piece.GetY() + yoffset))
        {
            if (IsEmpty(piece.GetX() + xoffset, piece.GetY() + yoffset))
            {
                SetNull(piece.GetX(), piece.GetY());
                int x = piece.GetX() + xoffset;
                int y = piece.GetY() + yoffset;
                piece.SetPosition(x, y);
                SetPosition(piece, piece.GetX(), piece.GetY());

                return true;
            }
            return false;
        }
        return false;
    }
    private void SetNull(int x , int y)
    {
        gamematrix[x, y] = null;
    }
    private void SetPosition(BoardPiece piece, int x, int y)
    {
        gamematrix[x, y] = piece;
    }

    private void CheckRecipe(BoardPiece piece)
    {
        List<BoardPiece> playerCooked = new List<BoardPiece>();
        playerCooked.Add(piece);
        int x = piece.GetX();
        int y = piece.GetY();

       // Debug.Log("x = " + x + ", y = " + y);
        while(IsValidPosition(x + 1, y))
        {
            ++x;
            playerCooked.Add(gamematrix[x, y]);
        }

        

        List<string> list1 = new List<string>();

        foreach (BoardPiece p in playerCooked)
        {
            list1.Add(p.name);
        }

        List<string> list2 = new List<string>();
        List<string> temp = new List<string>(list1);
        
        foreach (Recipe recipe in activeOrders)
        {
            list2 = recipe.GetIngridients();
            while (temp.Count > 0)
            {

                if (utility.ListComparer(temp, list2))
                {
                    
                    //recipe found
                    Debug.Log(recipe.name + " made");
                    
                    //clear the recipe from the gameMatrix
                    for(int i = 0; i < temp.Count; i++)
                    {
                        SetNull(piece.GetX() + i, piece.GetY());
                    }
                    //clear recipe from active orders
                    activeOrders.Remove(activeOrders.Find(x => x.name == recipe.name));
                    foreach (Recipe r in activeOrders)
                    {
                        Debug.Log(r.name);
                    }
                    //add another order to the active order list
                     AddNextOrder();
                    //check for win condition i.e if 
                    CheckWinCondition();
                    return;
                }
              
                temp.RemoveAt(temp.Count - 1);
                //Debug.Log(list1.Count);
                
            }
            temp = new List<string>(list1);
           
        }

    }
    private bool IsValidPosition(int x, int y)
    {
        if (x < 0 || x >= ROW || y < 0 || y >= COLOUMN)
            return false;
        return true;
    }
    private bool IsEmpty(int x, int y)
    {
        if (gamematrix[x, y] == null)
            return true;
        return false;
    }
    private void NewGame()
    {
        GenerateOrders();
        GenerateInitialActiveOrders();
        ClearGameMatrix();
    }
    private void ClearGameMatrix()
    {
        for (int i = 0; i < gamematrix.GetLength(0); i++)
        {
            for (int j = 0; j < gamematrix.GetLength(1); j++)
            {
                gamematrix[i, j] = null;
            }
        }
    }
    
    private void GenerateOrders() 
    {
        for(int i = 0; i < maxOrders; i++)
        {
            orders.Add(Random.Range(0, recipes.RecipeCount()));
        }
    }
   
    private void GenerateInitialActiveOrders()
    {
        for(int  i = 0; i < maxActiveOrders; i++)
        {
            AddNextOrder();
        }
        foreach(Recipe recipe  in activeOrders)
        {
            Debug.Log(recipe.name);
        }
    }
    private void AddNextOrder()
    {
        if(ordernumber < orders.Count)
        {

        ++ordernumber;
        activeOrders.Add(recipes.list[orders[ordernumber]]);
        GenerateActiveOrderIngridients(recipes.list[orders[ordernumber]]);
        }
    }
    private void GenerateActiveOrderIngridients(Recipe recipe)
    {
        int index = 0;
        while (index < recipe.IngridientCount())
        {

            if (!activeOrderIngridients.Contains(recipe.ingridients[index]))
            {
                activeOrderIngridients.Add(recipe.ingridients[index]);
            }
            ++index;
        }
    }
    private void CheckWinCondition()
    {
        if(ordernumber == 5)
        {
            Debug.Log("Win");
        }
    }


}

