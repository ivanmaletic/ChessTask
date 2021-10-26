using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Piece c;
        Piece c1;
        Piece c2;
        int i, j;
        bool wk;
        bool bk;
        bool wlr;
        bool wrr;
        bool blr;
        bool brr;
        wk = BoardManager.Instance.whiteKingMoved;
        bk = BoardManager.Instance.blackKingMoved;
        wlr = BoardManager.Instance.whiteLeftRookMoved;
        wrr = BoardManager.Instance.whiteRightRookMoved;
        blr = BoardManager.Instance.blackLeftRookMoved;
        brr = BoardManager.Instance.blackRightRookMoved;
        // Top
        i = CurrentX - 1;
        j = CurrentY + 1;
        if(CurrentY < 7)
        {
            for(int k = 0; k < 3; k++)
            {
                if(i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Pieces[i, j];
                    if (c == null) r[i, j] = true;
                    else if (c.isWhite != isWhite) r[i, j] = true;
                }
                i++;
            } 
        }

        // Bottom
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY > 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8)
                {
                    c = BoardManager.Instance.Pieces[i, j];
                    if (c == null) r[i, j] = true;
                    else if (c.isWhite != isWhite) r[i, j] = true;
                }
                i++;
            }
        }

        // Left
        if(CurrentX > 0)
        {
            c = BoardManager.Instance.Pieces[CurrentX - 1, CurrentY];
            if (c == null) r[CurrentX - 1, CurrentY] = true;
            else if (c.isWhite != isWhite) r[CurrentX - 1, CurrentY] = true;
        }

        // Right
        if (CurrentX < 7)
        {
            c = BoardManager.Instance.Pieces[CurrentX + 1, CurrentY];
            if (c == null) r[CurrentX + 1, CurrentY] = true;
            else if (c.isWhite != isWhite) r[CurrentX + 1, CurrentY] = true;
        }
        if (!wk&&!wlr)
        {   
            c = BoardManager.Instance.Pieces[CurrentX -2, CurrentY];
            c1 = BoardManager.Instance.Pieces[CurrentX -1, CurrentY];
            c2 = BoardManager.Instance.Pieces[CurrentX -3, CurrentY];
            if (c == null&&c1==null&&c2==null) r[CurrentX - 2, CurrentY] = true;
        }

        if (!wk&&!wrr)
        {   
            c = BoardManager.Instance.Pieces[CurrentX +2, CurrentY];
            c1 = BoardManager.Instance.Pieces[CurrentX +1, CurrentY];
            if (c == null&&c1==null) r[CurrentX + 2, CurrentY] = true;
        }

        if (!bk&&!blr)
        {   
            c = BoardManager.Instance.Pieces[CurrentX -2, CurrentY];
            c1 = BoardManager.Instance.Pieces[CurrentX -1, CurrentY];
            c2 = BoardManager.Instance.Pieces[CurrentX -3, CurrentY];
            if (c == null&&c1==null&&c2==null) r[CurrentX - 2, CurrentY] = true;
        }

        if (!bk&&!brr)
        {   c = BoardManager.Instance.Pieces[CurrentX +2, CurrentY];
            c1 = BoardManager.Instance.Pieces[CurrentX +1, CurrentY];
            if (c == null&&c1==null) r[CurrentX + 2, CurrentY] = true;
        }

        return r;
    }
}
