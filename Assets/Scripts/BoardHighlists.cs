using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlists : MonoBehaviour
{
    public static BoardHighlists Instance{set;get;}
    
    public GameObject highlightPrefab;
    private List<GameObject> highlists;

    private void Start()
    {
        Instance = this;
        highlists=new List<GameObject>();
    }

    private GameObject GetHighlightObject()
    {
        GameObject go = highlists.Find(g=>!g.activeSelf);
        if (go==null) 
        {
            go=Instantiate (highlightPrefab);
            highlists.Add(go);
        }
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }
    public void Hidehighlights()
    {
        foreach (GameObject go in highlists) 
        {
            go.SetActive(false);
        }
    }
}
   
