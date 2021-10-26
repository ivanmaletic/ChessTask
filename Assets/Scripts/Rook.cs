using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Piece c;
        int i;

        // Left
        i = CurrentX;
        while(true)
        {
            i--;
            if (i < 0) break;
            c = BoardManager.Instance.Pieces[i, CurrentY];
            if (c == null) r[i, CurrentY] = true;
            else
            {
                if(c.isWhite != isWhite) r[i, CurrentY] = true;
                break;
            }
        }

        // Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8) break;
            c = BoardManager.Instance.Pieces[i, CurrentY];
            if (c == null) r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite) r[i, CurrentY] = true;
                break;
            }
        }

        // Forward
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8) break;
            c = BoardManager.Instance.Pieces[CurrentX, i];
            if(c == null) r[CurrentX, i] = true;
            else
            {
                if(c.isWhite != isWhite) r[CurrentX, i] = true;
                break;
            }
        }

        // Back
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0) break;
            c = BoardManager.Instance.Pieces[CurrentX, i];
            if (c == null) r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite) r[CurrentX, i] = true;
                break;
            }
        }

        return r;
    }
}
