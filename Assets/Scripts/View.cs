using UnityEngine;
using Unity.Mathematics;
using TMPro;

public class View : MonoBehaviour
{
    [SerializeField] GameObject squarePrefab;

    [SerializeField] Transform gridParent;

    Controller controller;

    SquareView[,] gridView;

    void Awake()
    {
        controller = new Controller(this);
    }

    // Update is called once per frame
    public void CreateGrid(ref Board board, int rows, int cols)
    {
        gridView = new SquareView[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                gridView[i,j] = Instantiate(squarePrefab, gridParent).GetComponent<SquareView>();
                int2 coor = board.GetSquare(i, j).Coor;
                //newSquare.GetComponentInChildren<TextMeshProUGUI>().text = $"{coor.x},{coor.y}";
                gridView[i, j].SetSquare(coor.x, coor.y, this);
            }
        }
    }

    public void AddPiece(ref Piece piece, int2 coor)
    {
        gridView[coor.x, coor.y].AddPiece(ref piece);
    }

    public void RemovePiece(int2 coor) 
    {
        gridView[coor.x, coor.y].RemovePiece();
    }

    public void SelectSquare(int2 gridPos)
    {
        //Debug.Log(gridPos);
        controller.SelectSquare(gridPos);
    }
}

//Solo manda a llamar al controller