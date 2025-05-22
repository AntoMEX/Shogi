using UnityEngine;

using System;
using Unity.Mathematics;
using NUnit.Framework;
using System.Collections.Generic;

public class Controller
{
    View view;
    Board board;
    Piece selectedPiece = null;

    const int ROWS = 9;
    const int COLS = 9;

    Team curentTurn = Team.White;

    //public Board BOARD => board;

    Player whitePlayer;
    Player blackPlayer;

    List<int2> validMoves = new List<int2>();

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
            if (selectedSquare.piece == null) //Mover
            {
                if(!IsValidMove(selectedSquare.Coor) && selectedPiece.coor.x >= 0) return;
                if(selectedPiece.coor.x < 0)
                {
                    UpdateCemetaryCount(selectedPiece.type);
                }
                MoveSelectedPiece(selectedSquare);
                SwitchTeam();
            }
            else if (selectedSquare.piece.team == curentTurn) //Cambia seleccion
            {
                if (selectedPiece.coor.x < 0)
                {
                    EatPiece(ref selectedPiece);
                }
                SelectNewPiece (selectedSquare.piece);
            }
            else if(selectedPiece.coor.x >= 0) //Comer
            {
                if (!IsValidMove(selectedSquare.Coor)) return;
                EatPiece(ref selectedSquare.piece);
                MoveSelectedPiece(selectedSquare);
                SwitchTeam();
            }
        }
        else //Seleccion
        {
            if(selectedSquare.piece == null) return;
            if(selectedSquare.piece.team != curentTurn) return;
            SelectNewPiece (selectedSquare.piece);
        }
    }

    bool IsValidMove(int2 move)
    {
        foreach(int2 validMove in validMoves)
        {
            if (move.x != validMove.x) continue;
            if (move.y == validMove.y) return true;
        }
        return false;
    }

    void SelectNewPiece(Piece piece)
    {
        selectedPiece = piece;
        validMoves.Clear();
        List<int2> pieceMoves = selectedPiece.GetMoves();
        int2 pieceCoor = selectedPiece.coor;

        if (selectedPiece.GetType().IsSubclassOf(typeof(SingleMovePiece)))
        {
            foreach (int2 move in pieceMoves)
            {
                int2 newCoor;
                newCoor.x = move.x;
                newCoor.y = curentTurn == Team.White ? move.y : -move.y;
                newCoor += pieceCoor;
                if (newCoor.x < 0 || newCoor.x >= ROWS) continue;
                if (newCoor.y < 0 || newCoor.y >= ROWS) continue;
                if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                {
                    if (board.GetSquare(newCoor.x, newCoor.y).piece.team == curentTurn) continue;
                }
                validMoves.Add(newCoor);
            }
        }
        else if (selectedPiece.GetType().IsSubclassOf(typeof(DirectionalMovePiece)))
        {
            foreach (int2 direction in pieceMoves)
            {
                for (int i = 1; i <= 8; i++)
                {
                    int2 newCoor = pieceCoor + direction * i;
                    if (newCoor.x < 0 || newCoor.x >= ROWS) break;
                    if (newCoor.y < 0 || newCoor.y >= ROWS) break;
                    if (board.GetSquare(newCoor.x, newCoor.y).piece != null)
                    {
                        if (board.GetSquare(newCoor.x, newCoor.y).piece.team == curentTurn) break;
                        validMoves.Add(newCoor);
                        break;
                    }
                    validMoves.Add(newCoor);
                }
            }
        }
    }

    public void SelectCemetarySquare(PieceType pieceType)
    {
        Player currentPlayer = curentTurn == Team.White ? whitePlayer : blackPlayer;
        selectedPiece = pieceType switch
        {
            PieceType.Pawn => currentPlayer.sideBoard.pawns.Dequeue(),
            PieceType.Spear => currentPlayer.sideBoard.spears.Dequeue(),
            PieceType.Horse => currentPlayer.sideBoard.horses.Dequeue(),
            PieceType.Bishop => currentPlayer.sideBoard.bishops.Dequeue(),
            PieceType.Tower => currentPlayer.sideBoard.towers.Dequeue(),
            PieceType.Silver => currentPlayer.sideBoard.silvers.Dequeue(),
            PieceType.Gold => currentPlayer.sideBoard.golds.Dequeue(),
            _ => null
        };

    }

    void UpdateCemetaryCount(PieceType pieceType)
    {
        Player currentPlayer = curentTurn == Team.White ? whitePlayer : blackPlayer;

        switch (pieceType)
        {
            case PieceType.Pawn:
                //currentPlayer.sideBoard.pawns.Enqueue((Pawn)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.pawns.Count);
                break;
            case PieceType.Spear:
                //currentPlayer.sideBoard.spears.Enqueue((Spear)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.spears.Count);
                break;
            case PieceType.Horse:
                //currentPlayer.sideBoard.horses.Enqueue((Horse)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.horses.Count);
                break;
            case PieceType.Bishop:
                //currentPlayer.sideBoard.bishops.Enqueue((Bishop)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.bishops.Count);
                break;
            case PieceType.Tower:
                //currentPlayer.sideBoard.towers.Enqueue((Tower)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.towers.Count);
                break;
            case PieceType.Silver:
                //currentPlayer.sideBoard.silvers.Enqueue((Silver)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.silvers.Count);
                break;
            case PieceType.Gold:
                //currentPlayer.sideBoard.golds.Enqueue((Gold)eatenPiece);
                view.UpdateCemetary(curentTurn, pieceType, currentPlayer.sideBoard.golds.Count);
                break;
        }
    }

    void EatPiece(ref Piece eatenPiece)
    {
        eatenPiece.coor = new int2(-1, -1);
        eatenPiece.team = curentTurn;
        Player currentPlayer = curentTurn == Team.White ? whitePlayer : blackPlayer;

        switch (eatenPiece.type)
        {
            case PieceType.Pawn:
                currentPlayer.sideBoard.pawns.Enqueue((Pawn)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.pawns.Count);
                break;
            case PieceType.Spear:
                currentPlayer.sideBoard.spears.Enqueue((Spear)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.spears.Count);
                break;
            case PieceType.Horse:
                currentPlayer.sideBoard.horses.Enqueue((Horse)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.horses.Count);
                break;
            case PieceType.Bishop:
                currentPlayer.sideBoard.bishops.Enqueue((Bishop)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.bishops.Count);
                break;
            case PieceType.Tower:
                currentPlayer.sideBoard.towers.Enqueue((Tower)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.towers.Count);
                break;
            case PieceType.Silver:
                currentPlayer.sideBoard.silvers.Enqueue((Silver)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.silvers.Count);
                break;
            case PieceType.Gold:
                currentPlayer.sideBoard.golds.Enqueue((Gold)eatenPiece);
                view.UpdateCemetary(curentTurn, eatenPiece.type, currentPlayer.sideBoard.golds.Count);
                break;
        }
    }

    void MoveSelectedPiece(Square selectedSquare)
    {
        if (selectedPiece.coor.x >= 0) RemovePiece(selectedPiece.coor);
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

    void SwitchTeam()
    {
        curentTurn = curentTurn == Team.White ? Team.Black : Team.White;
        view.EnableTeamCemetary(curentTurn);
    }
    ~Controller()
    {

    }
}
