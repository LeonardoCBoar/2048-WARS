using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using UnityEngine.SceneManagement;



public class gameManager : MonoBehaviour
{

    public GameObject block;
    public GameObject socketHolder;


    
    
    // Start is called before the first frame update
    void Start()
    {


        generateColors();

    }


    Color[] colorList = new Color[20];
    void generateColors()
    {
        //geração de cores aleatórias pro tabuleiro e blocos
        for (int i = 0; i < colorList.Length; i++)
        {
            colorList[i] = new Color(
                Random.Range(0.1f, 1f),
                Random.Range(0.1f, 1f),
                Random.Range(0.1f, 1f)
                                    );
        }
        Color background = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
                                );
        GameObject.Find("Background").GetComponent<SpriteRenderer>().color = background;
    }


    public void updatePts(int pts,Text label)
    {
        label.text = string.Format("Points: {0}", pts.ToString());
    }


    public void renderBoard( int [,] board, Transform user)
    {
        float offset = 0.6f * user.localScale.x;
        float blockSize = 1.5f * user.localScale.x;
        Transform boardObject = user.GetChild(0).transform;
        for (int i = 1; i < boardObject.childCount; i++)
        {
            Destroy(boardObject.GetChild(i).gameObject);
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] < 0) board[i, j] *= -1;
                if (board[i, j] != 0)
                {
                    int value = (int)System.Math.Pow(2, board[i, j]);
                    var a = Instantiate(block, boardObject.position + new Vector3(offset + i * blockSize, -offset + j * -blockSize, 1), Quaternion.identity, boardObject);
                    a.transform.GetChild(0).GetComponent<TextMeshPro>().text = value.ToString();
                    a.transform.GetComponent<SpriteRenderer>().color = colorList[board[i, j]];
                }
            }
        }
    }

    
}

