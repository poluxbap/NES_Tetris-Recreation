using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int[] statistics = new int[7];
    public NumbersManager[] statisticsManager;

    public int lines;
    public NumbersManager linesManager;

    public int top;

    public int score;

    public int level;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddLines(int amount)
    {
        lines += amount;
        linesManager.ChangeNumber(lines);
    }

    public void AddStatistics(int amount, int tetromino)
    {
        statistics[tetromino] += amount;
        statisticsManager[tetromino].ChangeNumber(statistics[tetromino]);
    }
}
