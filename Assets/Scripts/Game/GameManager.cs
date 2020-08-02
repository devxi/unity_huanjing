using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform PlatformContainer;

    [HideInInspector]
    public AudioSource GameAudioSource;

    public GameStage CurGameStage => curGameStage;

    public int GameScore => gameScore;

    public int DiamondAmount => diamondAmount;

    private GameStage curGameStage;
    private int gameScore;//单局分数
    private int diamondAmount;//单局获得的钻石数量
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        curGameStage = GameStage.None;
        PlatformContainer = GameObject.Find("PlatformContainer").transform;
        GameAudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond );
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    
    private void AddScore()
    {
        if (curGameStage != GameStage.Playing)
        {
            Debug.LogWarning("当前游戏状态不是Playing 不能增加分数");
            return;
        }
        gameScore++;
        PlayerData.Inst().UpdateTopScore(gameScore);
        // Debug.Log("增加分數，当前分数：" + gameScore);
        EventCenter.Broadcast<int>(EventDefine.UpdateScoreText, gameScore);
    }
    
    private void AddDiamond()
    {
        if (curGameStage != GameStage.Playing)
        {
            Debug.LogWarning("当前游戏状态不是Playing 不能增加钻石");
            return;
        }
        diamondAmount++;
        Debug.Log("增加钻石，当前钻石数量：" + diamondAmount);
        PlayerData.Inst().AddDiamond();
        EventCenter.Broadcast<int>(EventDefine.UpdateDiamondText, diamondAmount);
    }

    public void StartGame()
    {
        if (curGameStage == GameStage.Playing)
        {
            Debug.LogError("游戏正在进行中，不能重复开始游戏");
            return;
        }
        gameScore = 0;
        diamondAmount = 0;
        SetGameStage(GameStage.Playing);
        EventCenter.Broadcast(EventDefine.OnGameStart);
    }

    public void Gameover()
    {
        if (curGameStage == GameStage.Gameover)
        {
            Debug.LogError("游戏已经是结束状态，请勿重复调用 Gameover");
            return; 
        }
        else
        {
            SetGameStage(GameStage.Gameover);
            var canvas = GameObject.Find("Canvas");
            var go = Instantiate(vars.GameOverPanel, canvas.transform);
        }
    }

    public static bool SetGameStage(GameStage newStage)
    {
        if (Instance.curGameStage == newStage)
        {
            Debug.LogError("游戏阶段设置无效，目前已经是该阶段" + newStage);
            return false;
        }
        else
        {
            Debug.LogFormat("游戏阶段变更[{0}] -> [{1}]", Instance.curGameStage, newStage);
            Instance.curGameStage = newStage;
        }

        return true;
    }
}

