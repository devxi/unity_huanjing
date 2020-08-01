using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviourBase
{
    private Text txt_Retry;
    private Button btn_Home;
    private Button btn_Rank;
    private ManagerVars vars;
    private Text txt_Score;
    private Text txt_TopScore;
    private Text txt_DiamondAmount;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    void Init()
    {
        txt_Score = GetUI<Text>("txt_Score");
        txt_TopScore = GetUI<Text>("txt_TopScore");
        txt_DiamondAmount = GetUI<Text>("AddDiamond/img_Diamond/txt_Diamond");
        
        txt_Retry = GetUI<Text>("txt_Retry");
        btn_Home = GetUI<Button>("Btns/btn_Home");
        btn_Rank = GetUI<Button>("Btns/btn_Rank");
        
        txt_Retry.GetComponent<Button>().onClick.AddListener(Retry);
        btn_Home.onClick.AddListener(ReturnHome);
        btn_Rank.onClick.AddListener(ShowRank);
        
        txt_Score.text = GameManager.Instance.GameScore.ToString();
        txt_TopScore.text = "最高分:" + PlayerData.Inst().TopScore.ToString();
        txt_DiamondAmount.text = "+" + GameManager.Instance.DiamondAmount.ToString();
    }

    void ReturnHome()
    {
        SceneManager.LoadScene(0);
    }

    void Retry()
    {
        Debug.Log("点击了重试");
        var canvas = GameObject.Find("Canvas");
        Close();
        Instantiate(vars.GamePanel, canvas.transform);
    }

    void ShowRank()
    {
        Debug.Log("点击了排行榜");
    }

    void Close()
    {

        DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log(("销毁结算界面"));
    }
}
