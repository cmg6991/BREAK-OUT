using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public Vector2Int size = new Vector2Int(5, 5);
    public Vector2 offset = new Vector2(1.1f, 0.6f);
    public Gradient gradient;

    public bool[] brickPattern1D; // Inspector에서 수정 가능

    // 1차원 배열을 2차원처럼 접근
    public bool GetBrick(int x, int y)
    {
        return brickPattern1D[y * size.x + x];
    }

    public void InitBrickPattern()
    {
        if (brickPattern1D != null && brickPattern1D.Length > 0)
            return;

        brickPattern1D = new bool[size.x * size.y];

        if (stageName == "Stage1")
        {
            bool[,] temp = new bool[,] {
                {true, false, true, true, false},
                {true, true, true, true, true},
                {false, true, false, true, false},
                {true, true, true, false, true},
                {false, true, true, true, false}
            };

            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    brickPattern1D[j * size.x + i] = temp[j, i];
        }

        else if (stageName == "Stage2")
        {
            bool[,] temp = new bool[,] {
                {true, true, true, true, true},
                {false, true, false, true, false},
                {true, false, true, false, true},
                {true, true, false, true, true},
                {false, true, true, true, false}
            };

            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    brickPattern1D[j * size.x + i] = temp[j, i];
        }
    }
}
