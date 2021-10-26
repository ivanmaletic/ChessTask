using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        // Up / Left
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        // Up / Right
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        // Down / Left
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        // Down / Right
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Piece c;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Pieces[x, y];
            if (c == null) r[x, y] = true;
            else if (c.isWhite != isWhite) r[x, y] = true;
        }
    }
}
