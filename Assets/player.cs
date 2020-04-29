using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        nextMove();
    }

    // Update is called once per frame
    void Update()
    {//checagem da ação do jogador
        if (Input.GetKeyDown(KeyCode.Space)) nextMove();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) moveRight();
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) moveLeft();
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) moveUp();
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) moveDown();
        else if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(1);
        //else if (Input.GetKeyDown(KeyCode.C)) generateColors();
    }


    //Funções utilitárias
    private void newBlock(int level)
    {//geração de novo bloco a cada jogada
        if (level > 15) return;
        level++;
        int value;
        if (UnityEngine.Random.Range(0, 5) != 4) value = 1;
        else value = 2;

        int x = UnityEngine.Random.Range(0, 4);
        int y = UnityEngine.Random.Range(0, 4);
        if (board[x, y] != 0)
        {
            newBlock(level);
            return;
        }
        else
        {
            board[x, y] = value;
            points += value;
            return;
        }
    }
    private bool checkLose()
    {//checa se não há movimentos possíveis pro usuário

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0) return false;
                else if (i > 0 && board[i - 1, j] == board[i, j]) return false;
                else if (i < 3 && board[i + 1, j] == board[i, j]) return false;
                else if (j > 0 && board[i, j - 1] == board[i, j]) return false;
                else if (j < 3 && board[i, j + 1] == board[i, j]) return false;
            }
        }
        return true;
    }

    int[,] board = new int[4, 4];
    int points = 0;

    //funções de movimentação
    private void moveUp()
    {//move todos os blocos para cima e passa o turno
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {//fors aninhados iteram pela matriz
                for (int t = 1; t <= j; t++)
                {
                    if (board[i, j] == 0)//checa se há bloco para mover
                    {
                        break; ;
                    }
                    else if (board[i, j] == board[i, j - t])//checa se há algum bloco para juntar
                    {
                        points += board[i, j] * board[i, j];
                        board[i, j - t] = (board[i, j - t] + 1) * -1;
                        board[i, j] = 0;
                        break;
                    }
                    else if (board[i, j - t] != board[i, j] && board[i, j - t] != 0)//checa se há algum bloco para colidir
                    {
                        if (board[i, j - t + 1] != board[i, j])
                        {
                            board[i, j - t + 1] = board[i, j];
                            board[i, j] = 0;
                        }
                        break;
                    }
                    else if (j - t == 0 && board[i, j] != 0)
                    {//caso não haja, colide com a parede
                        board[i, j - t] = board[i, j];
                        board[i, j] = 0;
                    }

                }

            }
        }
        nextMove();
    }
    private void moveDown()
    {//move todos os blocos para baixo e passa o turno
        for (int i = 0; i < 4; i++)
        {
            for (int j = 2; j >= 0; j--)
            {//fors aninhados iteram pela matriz
                for (int t = 1; t <= 3 - j; t++)
                {
                    if (board[i, j] == 0)//checa se há bloco para mover
                    {
                        break;
                    }
                    else if (board[i, j + t] == board[i, j])//checa se há algum bloco para juntar
                    {
                        points += board[i, j] * board[i, j];
                        board[i, j + t] = (board[i, j + t] + 1) * -1;
                        board[i, j] = 0;
                        break;
                    }
                    else if (board[i, j + t] != board[i, j] && board[i, j + t] != 0)//checa se há algum bloco para colidir
                    {
                        if (board[i, j] != board[i, j + t - 1])
                        {
                            board[i, j + t - 1] = board[i, j];
                            board[i, j] = 0;
                        }
                        break;
                    }
                    else if (j + t == 3)//caso não haja, colide com a parede
                    {
                        board[i, j + t] = board[i, j];
                        board[i, j] = 0;
                    }

                }

            }
        }
        nextMove();
    }

    private void moveLeft()
    {//move todos os blocos para esquerda e passa o turno
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {//fors aninhados iteram pela matriz

                for (int t = 1; t <= i; t++)
                {
                    if (board[i, j] == 0)//checa se há bloco para mover
                    {
                        break;
                    }
                    else if (board[i - t, j] == board[i, j])//checa se há algum bloco para juntar
                    {
                        points += board[i, j] * board[i, j];
                        board[i - t, j] = (board[i - t, j] + 1) * -1;
                        board[i, j] = 0;
                        break;
                    }
                    else if (board[i - t, j] != board[i, j] && board[i - t, j] != 0)//checa se há algum bloco para colidir
                    {
                        if (board[i, j] != board[i - t + 1, j])
                        {
                            board[i - t + 1, j] = board[i, j];
                            board[i, j] = 0;
                        }
                        break;
                    }
                    else if (i - t == 0)//caso não haja, colide com a parede
                    {
                        board[i - t, j] = board[i, j];
                        board[i, j] = 0;
                    }

                }

            }
        }

        nextMove();


    }
    private void moveRight()
    {//move todos os blocos para a direita e passa o turno
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 0; j < 4; j++)
            {

                for (int t = 1; t <= 3 - i; t++)
                {
                    if (board[i, j] == 0)//checa se há bloco para mover
                    {
                        break;
                    }
                    else if (board[i + t, j] == board[i, j])//checa se há algum bloco para juntar
                    {
                        points += board[i, j] * board[i, j];
                        board[i + t, j] = (board[i + t, j] + 1) * -1;
                        board[i, j] = 0;
                        break;
                    }
                    else if (board[i + t, j] != board[i, j] && board[i + t, j] != 0)//checa se há algum bloco para colidir
                    {
                        if (board[i, j] != board[i + t - 1, j])
                        {
                            board[i + t - 1, j] = board[i, j];
                            board[i, j] = 0;
                        }
                        break;
                    }
                    else if (i + t == 3)//caso não haja, colide com a parede
                    {
                        board[i, j] = 0;
                        board[i + t, j] = 2;
                    }

                }

            }
        }
        nextMove();
    }



    public GameObject manager;

    public GameObject socketHolder;
    private void nextMove()
    {//Avança uma rodada no jogo
        newBlock(0);
        manager.GetComponent<gameManager>().updatePts(points, transform.GetChild(1).GetComponentInChildren<Text>());
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        manager.GetComponent<gameManager>().renderBoard(board,transform);

        //envio de dados para o outro jogador
        socketHolder.GetComponent<clientSender>().protocolBoard(board,points);
        if (checkLose())
        {
            print("Eu perdi...");
        }
        stopWatch.Stop();
        //TEMPO DE EXECUÇÃO FUNÇÃOprint(stopWatch.Elapsed);
    }

}
