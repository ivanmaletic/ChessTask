using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{

    /*public GameObject[] listOfPieces;
    public List<GameObject[]> listOfMoves=new List<GameObject[]>();*/

    public static BoardManager Instance{set;get;}

    private bool[,] allowedMoves{set;get;}

    private bool[,] mateAllowedMoves{set;get;}


    public Piece [,] Pieces{set;get;}
    private Piece selectedPiece;

    private const float TILE_SIZE =1.0f;
    private const float TILE_OFFSET=0.5f;

    private int selectionX=-1;
    private int selectionY=-1;

    private Material previousMat;
    public Material selectedMat;

    public List<GameObject> piecesPrefab;
    private List<GameObject> activePiece= new List<GameObject>();

    private Quaternion orientation = Quaternion.Euler(-90,-90,0);
    private Quaternion orientation1 = Quaternion.Euler(-90,90,0);

    public bool isWhiteTurn = true;

    public int[] EnPassant{set;get;}

    public bool whiteKingMoved{set;get;}
    public bool blackKingMoved{set;get;}

    public bool whiteLeftRookMoved{set;get;}

    public bool blackLeftRookMoved{set;get;}

    public bool whiteRightRookMoved{set;get;}

    public bool blackRightRookMoved{set;get;}



    int counter;
    private void Start()
    {
        blackKingMoved = false;
        whiteLeftRookMoved=false;
        blackLeftRookMoved=false;
        whiteRightRookMoved=false;
        blackRightRookMoved=false;
        whiteKingMoved=false;
        Instance=this;
        Pieces= new Piece[8,8];
        SpawnAllPieces();
    }

    void Update()
    {
       //DrawChessBoard();
       UpdateSelection();
       if (Input.GetMouseButtonDown(0)) 
       {
           if (selectionX >= 0 && selectionY>=0) 
           {
               if (selectedPiece==null) 
               {
                   SelectPiece(selectionX,selectionY);
               }
               else
               {
                   MovePiece(selectionX, selectionY);
               }
           }
       }

       
    }

    private void SpawnAllPieces()
    {
        EnPassant=new int[2]{-1,-1};
        //Spawn white team
        //White king
        SpawnPiece(0, 4,0);
        //White queen 
        SpawnPiece(1, 3,0);
        //White rooks
        SpawnPiece(2, 0,0);
        SpawnPiece(2, 7,0);
        //White bishoops
        SpawnPiece(3, 2,0);
        SpawnPiece(3, 5,0);
        //White knights
        SpawnPiece(4, 1,0);
        SpawnPiece(4, 6,0);
        // White pawns 
        for (int i = 0;i<8;i++) 
        {
            SpawnPiece(5, i,1);
        }


        //Spawn black team
        //Black king
        SpawnPiece(6, 4,7);
        //Black queen 
        SpawnPiece(7, 3,7);
        //Black rooks
        SpawnPiece(8, 0,7);
        SpawnPiece(8, 7,7);
        //Black bishoops
        SpawnPiece(9, 2,7);
        SpawnPiece(9,5,7);
        //Black knights
        SpawnPiece(10, 1,7);
        SpawnPiece(10, 6,7);
        // Black pawns 
        for (int j = 0;j<8;j++) 
        {
            SpawnPiece(11, j,6);
        }
    }

    private void SpawnPiece(int index, int x, int y)
    {
        if (index == 11||index ==5)
        {
            Vector3 posit=GetTileCenter(x,y);
            GameObject go = Instantiate(piecesPrefab[index],new Vector3(posit.x,0,posit.z), orientation1) as GameObject;
            go.transform.SetParent(transform);
            Pieces[x,y]=go.GetComponent<Piece>();
            Pieces[x,y].SetPosition(x,y);
            activePiece.Add(go);
        }

        //rotate the black knight
        else if (index == 10)
        {
            GameObject go = Instantiate(piecesPrefab[index], GetTileCenter(x,y), orientation1) as GameObject;
            go.transform.SetParent(transform);
            Pieces[x,y]=go.GetComponent<Piece>();
            Pieces[x,y].SetPosition(x,y);
            activePiece.Add(go);
        }
        else
        {
            GameObject go = Instantiate(piecesPrefab[index], GetTileCenter(x,y), orientation) as GameObject;
            go.transform.SetParent(transform);
            Pieces[x,y]=go.GetComponent<Piece>();
            Pieces[x,y].SetPosition(x,y);
            activePiece.Add(go);
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;

    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        //Drawing selection
        if (selectionX >= 0 && selectionY>=0) 
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX, Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX+1));
            Debug.DrawLine(Vector3.forward*(selectionY + 1)+Vector3.right * selectionX,Vector3.forward * selectionY+Vector3.right * (selectionX+1));
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (Pieces[x,y]==null) 
            return;
        if (Pieces[x,y].isWhite != isWhiteTurn)
            return;
        allowedMoves = Pieces[x,y].PossibleMove();
        bool hasAtLeastOneMove = false;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(allowedMoves[i,j])
                {
                    hasAtLeastOneMove = true;
                    i = 7;
                    break;
                }
            }
        }
        if (!hasAtLeastOneMove) return;

        selectedPiece=Pieces[x,y];


       CheckForCheck(selectedPiece,selectionX,selectionY);
        previousMat = selectedPiece.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedPiece.GetComponent<MeshRenderer>().material = selectedMat;

        BoardHighlists.Instance.HighlightAllowedMoves(allowedMoves); 
    }


    private void MovePiece(int x, int y)
    {
        if (allowedMoves[x,y]) 
        {
            Piece c = Pieces[x,y];
            Piece wlr = Pieces[0,0];
            Piece blr = Pieces[0,7];
            Piece wrr = Pieces[7,0];
            Piece brr = Pieces[7,7];
            if (selectedPiece.CurrentX==0&&selectedPiece.CurrentY==0) whiteLeftRookMoved = true;
            if (selectedPiece.CurrentX==7&&selectedPiece.CurrentY==0) whiteRightRookMoved=true;
            if (selectedPiece.CurrentX==0&&selectedPiece.CurrentY==7) blackLeftRookMoved=true;
            if (selectedPiece.CurrentX==7&&selectedPiece.CurrentY==7) blackRightRookMoved=true;

            //Castling
            if (selectedPiece.GetType() == typeof(King))
            {   
                if (x==2&&selectedPiece.isWhite) 
                {
                    activePiece.Remove(wlr.gameObject);
                    Destroy(wlr.gameObject);
                    SpawnPiece(2, 3,0);
                    activePiece.Add(Pieces[3,0].gameObject);
                }
                if (x==2&&!selectedPiece.isWhite) 
                {
                    activePiece.Remove(blr.gameObject);
                    Destroy(blr.gameObject);
                    SpawnPiece(8, 3,7);
                    activePiece.Add(Pieces[3,7].gameObject);
                }
                if (x==6&&selectedPiece.isWhite) 
                {
                    activePiece.Remove(wrr.gameObject);
                    Destroy(wrr.gameObject);
                    SpawnPiece(2, 5,0);
                    activePiece.Add(Pieces[5,0].gameObject);
                }
                if (x==6&&!selectedPiece.isWhite) 
                {
                    activePiece.Remove(brr.gameObject);
                    Destroy(brr.gameObject);
                    SpawnPiece(8, 5,7);
                    activePiece.Add(Pieces[5,7].gameObject);
                }

                if (selectedPiece.isWhite) whiteKingMoved=true;
                else blackKingMoved=true;
            }
                           
            if (c != null && c.isWhite != isWhiteTurn)
            {
                activePiece.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            Pieces[selectedPiece.CurrentX,selectedPiece.CurrentY] = null;
            if (x == EnPassant[0] && y == EnPassant[1]) 
            {
                if (isWhiteTurn)
                    c = Pieces[x,y-1];

                else 
                    c = Pieces[x,y+1];
                activePiece.Remove(c.gameObject);
                Destroy(c.gameObject);

            }
            Vector3 pos = GetTileCenter(x, y);
            selectedPiece.transform.position = pos;

            EnPassant[0]=-1;
            EnPassant[1]=-1;

            if (selectedPiece.GetType()==typeof(Pawn)) 
            {
                if (y==7) 
                {
                activePiece.Remove(selectedPiece.gameObject);
                Destroy(selectedPiece.gameObject);
                SpawnPiece(1,x,y);
                selectedPiece=Pieces[x,y];
                }

                else if (y==0) 
                {
                    activePiece.Remove(selectedPiece.gameObject);
                    Destroy(selectedPiece.gameObject);
                    SpawnPiece(7,x,y);
                    selectedPiece=Pieces[x,y];
                }

                if (selectedPiece.CurrentY == 1 && y == 3)
                {
                    EnPassant[0]=x;
                    EnPassant[1]=y-1;
                }
                else if (selectedPiece.CurrentY == 6 && y == 4)
                {
                    EnPassant[0]=x;
                    EnPassant[1]=y+1;
                }

                selectedPiece.transform.position = new Vector3(pos.x, 0, pos.z);

            }
            selectedPiece.SetPosition(x, y);
            Pieces[x,y] = selectedPiece;
            isWhiteTurn = !isWhiteTurn;
        }

        //MadeMove();
        selectedPiece.GetComponent<MeshRenderer>().material = previousMat;
        selectedPiece = null;
        counter=0;
        BoardHighlists.Instance.Hidehighlights();
        for (int k=0;k<8;k++)    
        {
           for (int l=0;l<8;l++) 
           {
               if (Pieces[k,l]!=null&&Pieces[k,l].isWhite == isWhiteTurn) 
               {
                   mateAllowedMoves = Pieces[k,l].PossibleMove();
                   CheckForMate(Pieces[k,l],k,l);

                   foreach (bool am in mateAllowedMoves)
                   {
                           if (am) 
                           {
                               counter++;
                           }
                   }
               }
               if (counter==0&&k==7&&l==7) 
               {
                   EndGame();
               }
           }
        }
    }
    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        float raycastDistance = 25.0f;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn) 
        {
            Debug.Log("Black wins");
        }
        else Debug.Log("White wins");
        foreach (GameObject go in activePiece)
        Destroy(go);
        isWhiteTurn = true;
        BoardHighlists.Instance.Hidehighlights();
        SpawnAllPieces();
        /*SaveToJson();
        LoadFromJson();*/
    }

    public void CheckForCheck(Piece selPie, int returnX, int returnY)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0;j<8;j++) 
            {
                if (allowedMoves[i,j]) 
                {  
                    Piece tempEnemyDisable=Pieces[i,j];
                    if (Pieces[i,j]!=null) 
                    {
                        tempEnemyDisable.transform.position = new Vector3(1000,0,1000);
                        Pieces[i,j]=null;
                    }

                    selPie.transform.position=GetTileCenter(i,j);
                    Pieces[i,j] = selPie;
                    Pieces[returnX,returnY]=null;
                    for (int k = 0; k < 8; k++) 
                    {
                        for (int l = 0; l < 8; l++)
                        {   

                            if (Pieces[k,l] != null && Pieces[k,l].isWhite != isWhiteTurn) 
                            {

                                if (Pieces[k,l].PossibleMove()[i,j] && selPie.GetType()==typeof(King))
                                    allowedMoves[i,j]=false;
                                for (int m = 0; m < 8; m++) 
                                {
                                    for (int n = 0;n<8;n++) 
                                    {
                                        if (Pieces[k,l].PossibleMove()[m,n]) 
                                        {
                                            Piece p = Pieces[m,n];
                                            if (p!=null&&p.GetType() == typeof(King)) 
                                            {
                                                allowedMoves[i,j]=false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    selPie.transform.position=GetTileCenter(returnX,returnY);
                    if(tempEnemyDisable!=null)tempEnemyDisable.transform.position = GetTileCenter(i,j);
                    Pieces[i,j] = tempEnemyDisable;
                    Pieces[returnX,returnY] = selPie;
                }
            }
        }
    }

    public void CheckForMate(Piece selPie, int returnX, int returnY)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0;j<8;j++) 
            {
                if (mateAllowedMoves[i,j]) 
                {   

                    Piece tempEnemyDisable = Pieces[i,j];
                    if (Pieces[i,j]!=null) 
                    {
                        tempEnemyDisable.transform.position = new Vector3(1000,0,1000);
                        Pieces[i,j]=null;
                    }

                    selPie.transform.position=GetTileCenter(i,j);
                    Pieces[returnX,returnY]=null;
                    for (int k = 0; k < 8; k++) 
                    {
                        for (int l = 0; l < 8; l++)
                        {   

                            if (Pieces[k,l] != null && Pieces[k,l].isWhite != isWhiteTurn) 
                            {
                                if (Pieces[k,l].PossibleMove()[i,j] && selPie.GetType()==typeof(King))
                                mateAllowedMoves[i,j]=false;
                                for (int m = 0; m < 8; m++) 
                                {
                                    for (int n = 0;n<8;n++) 
                                    {
                                        if (Pieces[k,l].PossibleMove()[m,n]) 
                                        {

                                            Piece p = Pieces[m,n];
                                            if (p!=null&&p.GetType() == typeof(King)) 
                                            {
                                                mateAllowedMoves[i,j]=false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    selPie.transform.position=GetTileCenter(returnX,returnY);
                    if(tempEnemyDisable!=null)tempEnemyDisable.transform.position = GetTileCenter(i,j);
                    Pieces[i,j] = tempEnemyDisable;
                    Pieces[returnX,returnY] = selPie;
                }
            }
        }
    }

/*    public void MadeMove()
    {
            listOfPieces = GameObject.FindGameObjectsWithTag("ChessPiece");
            foreach (GameObject go in listOfPieces) 
            {
                Debug.Log(go.transform.position);
            }
                listOfMoves.Add(listOfPieces);

    }

    public void SaveToJson()
    {
        WhatToSave whatToSave = new WhatToSave();
        whatToSave.gObject =listOfMoves[1].text;
        whatToSave.gPosition = "33";
        whatToSave.gRotation = "44";
 
        string json = JsonUtility.ToJson(whatToSave, true);
        File.WriteAllText(Application.dataPath + "/savedata.json", json);

    }

    public void LoadFromJson()
    {
         string json = File.ReadAllText(Application.dataPath + "/savedata.json");
         WhatToSave whatToSave = JsonUtility.FromJson<WhatToSave>(json);
         Debug.Log(whatToSave.gObject);
     }
*/
}
    
