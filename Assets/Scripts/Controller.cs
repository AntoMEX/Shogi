using UnityEngine;

using System;
using Unity.Mathematics;

public class Controller
{
    View view;
    Board board;

    const int ROWS = 9;
    const int COLS = 9;

    public Board BOARD => board;
    public Controller(View view)
    {
        this.view = view;
        board = new Board(ROWS, COLS);
        view.CreateGrid(ref board, ROWS, COLS);
        SetBoard();
        //view.RemovePiece(new int2(0, 1));

    }

    void SetBoard()
    {
        CreatePiece(new int2(2,6), PieceType.Pawn, Team.White);
        CreatePiece(new int2(1, 2), PieceType.Pawn, Team.Black);
    }

    void CreatePiece(int2 coor, PieceType type, Team team)
    {
        Piece piece = null;
        switch (type)
        {
            case PieceType.Pawn:
                piece = new Pawn(coor, team);
                break;
            case PieceType.Spear:
                piece = new Spear(coor, team);
                break;
            case PieceType.Horse:
                piece = new Horse(coor, team);
                break;
            case PieceType.Bishop:
                piece = new Bishop(coor, team);
                break;
            case PieceType.Tower:
                piece = new Tower(coor, team);
                break;
            case PieceType.Silver:
                piece = new Silver(coor, team);
                break;
            case PieceType.Gold:
                piece = new Gold(coor, team);
                break;
            case PieceType.King:
                piece = new King(coor, team);
                break;
            //default
        }
        if (piece == null) return;
        //Piece piece = new Piece(coor, type, team);
        board.GetSquare(coor.x, coor.y).piece = piece;
        view.AddPiece(ref piece, coor);
    }

    ~Controller()
    {

    }



}
