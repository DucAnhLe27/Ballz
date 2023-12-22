using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSpawnPoints : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] Bricks;
    [SerializeField] GameObject[] Powerup;
    [SerializeField] GameObject extraBallPowerup;

    [SerializeField] static readonly int colNum = 7;
    [SerializeField] static readonly int rowNum = 10;

    [SerializeField] private ArrayList bricksArray;

    [SerializeField] public static int level;

    [SerializeField] Transform Parent;
    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        bricksArray = new ArrayList();
        for (int row = 0; row < rowNum; row++)
        {
            ArrayList tmp = new ArrayList();
            for (int col = 0; col < colNum; col++)
            {
                GameObject b = null;
                tmp.Add(b);
            }
            bricksArray.Add(tmp);
        }
        PlaceBricks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaceBricks()
    {
        level++;
        int extraBallPos = Random.Range(0, colNum);
        GameObject extraBall = Instantiate(extraBallPowerup, spawnPoints[extraBallPos].position, Quaternion.identity);
        extraBall.transform.parent = Parent.transform;
        SetBrick(extraBallPos, rowNum - 2, extraBall);

        for (int i = 0; i < colNum; i++)
        {
            if (GetBrick(i, rowNum - 2) == null)
            {
                int brickToCreate = Random.Range(0, 10);
                if (brickToCreate < 5)
                {
                    GameObject brick = Instantiate(Bricks[0], spawnPoints[i].position, Quaternion.identity);
                    brick.transform.parent = Parent.transform;
                    SetBrick(i, rowNum - 1, brick);
                }
                else if (brickToCreate == 6)
                {
                    GameObject brick = Instantiate(Bricks[Random.Range(1, 5)], spawnPoints[i].position, Quaternion.identity);
                    brick.transform.parent = Parent.transform;
                    SetBrick(i, rowNum - 1, brick);
                }
                else if (brickToCreate == 7)
                {
                    GameObject brick = Instantiate(Powerup[Random.Range(0, 3)], spawnPoints[i].position, Quaternion.identity);
                    brick.transform.parent = Parent.transform;
                    SetBrick(i, rowNum - 1, brick);
                }
            }
        }
    }
    private GameObject GetBrick(int colIndex, int rowIndex)
    {
        ArrayList tmp = bricksArray[rowIndex] as ArrayList;
        GameObject c = tmp[colIndex] as GameObject;
        return c;
    }

    private void SetBrick(int colIndex, int rowIndex, GameObject c)
    {
        ArrayList tmp = bricksArray[rowIndex] as ArrayList;
        tmp[colIndex] = c;
    }

    public void BrickMove()
    {
        for (int row = 1; row < rowNum - 1; row++)
        {
            for (int col = 0; col < colNum; col++)
            {
                GameObject b = GetBrick(col, row);
                SetBrick(col, row - 1, b);
            }
        }
        for (int col = 0; col < colNum; col++)
        {
            SetBrick(col, rowNum - 1, null);
        }
    }

    public void ErrorBouncePowerup(int row)
    {
        while (true)
        {
            int col = Random.Range(0, colNum);
            GameObject b = GetBrick(col, row);
            if (b == null)
            {
                GameObject brick = Instantiate(Powerup[2], new Vector3(col - 3f, (float)row - 3.5f, 0), Quaternion.identity);
                SetBrick(col, row, brick);
                return;
            }
        }
    }
}
