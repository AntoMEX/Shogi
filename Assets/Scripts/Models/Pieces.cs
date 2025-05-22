using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

public enum PieceType
{
    Pawn,
    Spear,
    Horse,
    Silver,
    Gold,
    Tower,
    Bishop,
    King
}
public enum Team
{
    White,
    Black
}

public abstract class Piece
{
    public int2 coor;
    public PieceType type;
    public Team team;
    public List<int2> moves;

    public Piece(int2 coor, PieceType type, Team team)
    {
        this.coor = coor;
        this.type = type;
        this.team = team;
    }

    public abstract List<int2> GetMoves();
}

public abstract class SingleMovePiece : Piece 
{
    protected List<int2> moves = new List<int2>();
    public SingleMovePiece(int2 coor, PieceType type, Team team) : base(coor, type, team) { }

    public override List<int2> GetMoves()
    {
        return moves;
    }
}

public abstract class DirectionalMovePiece : Piece
{
    protected List<int2> directions = new List<int2>();
    public DirectionalMovePiece(int2 coor, PieceType type, Team team) : base (coor, type, team) { }

    public override List<int2> GetMoves()
    {
        return directions;
    }
}
//Peon
public class Pawn : SingleMovePiece
{
    public Pawn(int2 coor, Team team) : base(coor, PieceType.Pawn, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Pawn;
        this.team = team;*/
        moves = new List<int2>()
        {
            new int2(0, -1),
        };
    }
}
//Lanza
public class Spear : DirectionalMovePiece
{
    public Spear(int2 coor, Team team) : base(coor, PieceType.Spear, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Spear;
        this.team = team;*/
        directions = new List<int2>()
        {
            new int2(-1, 0),
        };
    }
    
}
//Caballo
public class Horse : SingleMovePiece
{
    public Horse(int2 coor, Team team) : base(coor, PieceType.Horse, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Horse;
        this.team = team;*/
        moves = new List<int2>()
        {
            new int2(-1, -2),
            new int2(1, -2),
        };
    }
}
//Alfil
public class Bishop : DirectionalMovePiece
{
    public Bishop(int2 coor, Team team) : base(coor, PieceType.Bishop, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Bishop;
        this.team = team;*/
        directions = new List<int2>()
        {
            new int2(-1, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
    }
}
//Torre
public class Tower : DirectionalMovePiece
{
    public Tower(int2 coor, Team team) : base(coor, PieceType.Tower, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Tower;
        this.team = team;*/
        directions = new List<int2>()
        {
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, -1),
            new int2(0, 1),
        };
    }
}
//Plateado
public class Silver : SingleMovePiece
{
    public Silver(int2 coor, Team team) : base(coor, PieceType.Silver, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Silver;
        this.team = team;*/
        moves = new List<int2>()
        {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 1),
            new int2(1, 1),
        };
    }
}
//Dorado
public class Gold : SingleMovePiece
{
    public Gold(int2 coor, Team team) : base(coor, PieceType.Gold, team)
    {
        /*this.coor = coor;
        this.type = PieceType.Gold;
        this.team = team;*/
        moves = new List<int2>()
        {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 0),
            new int2(1, 0),
            new int2(0, 1),
        };
    }
}
//Rey
public class King : SingleMovePiece
{
    public King(int2 coor, Team team) : base(coor, PieceType.King, team)
    {
        /*this.coor = coor;
        this.type = PieceType.King;
        this.team = team;*/

        moves = new List<int2>()
        {
            new int2(-1, -1),
            new int2(0, -1),
            new int2(1, -1),
            new int2(-1, 0),
            new int2(1, 0),
            new int2(-1, 1),
            new int2(0, 1),
            new int2(1, 1),
        };
    }
}

