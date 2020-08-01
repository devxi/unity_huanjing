
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlayerData
{
    private static PlayerData _inst;
    public int DiamondAmount { get; set; }  //钻石数量
    public int TopScore => topScore; //最高分记录

    private int topScore;
    
    
    
    public static PlayerData Inst()
    {
        if (_inst == null)
        {
            _inst = new PlayerData();
        }

        return _inst;
    }

    public void AddDiamond()
    {
        DiamondAmount++;
    }

    public bool UpdateTopScore(int newScore)
    {
        if (newScore > topScore)
        {
            topScore = newScore;
            return true;
        }
        return false;
    }
}
