using UnityEngine;

using System;
using Unity.Mathematics;

public class Controller
{
    View view;
    Board board;
    Piece selectedPiece;

    const int ROWS = 9;
    const int COLS = 9;

    Team curentTurn = Team.White;

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
        //Peones
        for (int i = 0; i < ROWS; i++)
        {
            CreatePiece(new int2(i, 6), PieceType.Pawn, Team.White);
            CreatePiece(new int2(i, 2), PieceType.Pawn, Team.Black);
        }

        //Lanzas
        CreatePiece(new int2(0, 8), PieceType.Spear, Team.White);
        CreatePiece(new int2(8, 8), PieceType.Spear, Team.White);
        CreatePiece(new int2(0, 0), PieceType.Spear, Team.Black);
        CreatePiece(new int2(8, 0), PieceType.Spear, Team.Black);

        //Caballos
        CreatePiece(new int2(1, 8), PieceType.Horse, Team.White);
        CreatePiece(new int2(7, 8), PieceType.Horse, Team.White);
        CreatePiece(new int2(1, 0), PieceType.Horse, Team.Black);
        CreatePiece(new int2(7, 0), PieceType.Horse, Team.Black);

        //Alfiles
        CreatePiece(new int2(1, 7), PieceType.Bishop, Team.White);
        CreatePiece(new int2(7, 1), PieceType.Bishop, Team.Black);

        //Torres
        CreatePiece(new int2(7, 7), PieceType.Tower, Team.White);
        CreatePiece(new int2(1, 1), PieceType.Tower, Team.Black);

        //Plateados
        CreatePiece(new int2(2, 8), PieceType.Silver, Team.White);
        CreatePiece(new int2(6, 8), PieceType.Silver, Team.White);
        CreatePiece(new int2(2, 0), PieceType.Silver, Team.Black);
        CreatePiece(new int2(6, 0), PieceType.Silver, Team.Black);

        //Dorados
        CreatePiece(new int2(3, 8), PieceType.Gold, Team.White);
        CreatePiece(new int2(5, 8), PieceType.Gold, Team.White);
        CreatePiece(new int2(3, 0), PieceType.Gold, Team.Black);
        CreatePiece(new int2(5, 0), PieceType.Gold, Team.Black);

        //Reyes
        CreatePiece(new int2(4, 8), PieceType.King, Team.White);
        CreatePiece(new int2(4, 0), PieceType.King, Team.Black);
    }

    public void SelectSquare(int2 gridPos)
    {
        ref Square selectedSquare = ref board.GetSquare(gridPos.x, gridPos.y); //referencia de Square, no copia
        if(selectedPiece != null)
        {
            if (selectedSquare.piece == null)
            {
                MoveSelectedPiece(selectedSquare);
            }
            else if (selectedSquare.piece.team == curentTurn)
            {
                selectedPiece = selectedSquare.piece;
            }
            else
            {
                EatPiece(selectedSquare.Coor);
                MoveSelectedPiece(selectedSquare);
            }
        }
        else
        {
            if(selectedSquare.piece == null) return;
            if(selectedSquare.piece.team != curentTurn) return;
            selectedPiece = selectedSquare.piece;
        }
    }

    void EatPiece(int2 coor)
    {
        
    }

    private void MoveSelectedPiece(Square selectedSquare)
    {
        RemovePiece(selectedPiece.coor);
        AddPiece(ref selectedPiece, selectedSquare.Coor);
        selectedPiece = null;
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

    void RemovePiece(int2 coor)
    {
        board.GetSquare(coor.x, coor.y).piece = null;
        view.RemovePiece(coor);
    }

    void AddPiece(ref Piece piece, int2 coor)
    {
        board.GetSquare(coor.x, coor.y).piece = piece;
        piece.coor = coor;
        view.AddPiece(ref piece, coor);
    }

    ~Controller()
    {

    }
}
