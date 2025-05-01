
using Unity.Mathematics;
using UnityEngine;


public struct Board
{
    //const int ROWS = 9;
    //const int COLS = 9;

    Square[,] grid; //Arreglo bidimiensional
    //Square[][] grid2; Arreglo de arreglos
    public Board(int rows, int cols) 
    { 
        grid = new Square[rows, cols];
        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = new Square(i, j);
            }
        }
        //Debug.Log(grid[3, 0].GetCoor);
    }
    public ref Square GetSquare(int row, int col) => ref grid[row, col]; //Pide row y column y regresa la copia del square

    //~Board() {}
}

public struct Square
{
    int2 coor; //Unity.Mathematics  enteros bidimensionales
    //float2 coor; Unity.Mathematics  flotantes bidimensionales

    public Piece piece;

    //public int2 GetCoor => coor;
    public Square(int x, int y) 
    { 
        coor = new int2(x, y);
        piece = null;
    }

    //~Square() {}
    public int2 Coor => coor;
}