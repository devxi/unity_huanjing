using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviourBase
{
    private Button btn_Pause;
    private Text txt_Score;
    private Text txt_DiamondCount;
    private Button btn_play;

    private void Awake()
    {
        GameManager.Instance.StartGame();
        btn_Pause = GetUI<Button>("btn_Pause");
        btn_play = GetUI<Button>("btn_Play");
        txt_Score = GetUI<Text>("txt_Score");
        txt_DiamondCount = GetUI<Text>("txt_DiamondCount");
        
        btn_play.gameObject.SetActive(false);
        
        btn_play.onClick.AddListener(OnClickPlay);
        btn_Pause.onClick.AddListener(OnClickPause);
        Instantiate(ManagerVars.GetManagerVars().Player);
        
        EventCenter.AddListener(EventDefine.CloseGamePanle, Close);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreTxt);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondTxt);
        
        
    }

    private void OnClickPause()
    {
        btn_Pause.gameObject.SetActive(false);
        btn_play.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    private void OnClickPlay()
    {
        btn_Pause.gameObject.SetActive(true);
        btn_play.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
    
    void Close()
    {       
        Debug.Log(("销毁游戏界面"));
        var platformContainer = GameManager.Instance.PlatformContainer;
        for (int i = 0; i < platformContainer.childCount; i++)
        {
            Destroy(platformContainer.GetChild(i).gameObject);
        }
        Destroy(gameObject);
        GameManager.Instance.Gameover();
    }

    void UpdateScoreTxt(int score)
    {
        txt_Score.text = score.ToString();
    }
    
    void UpdateDiamondTxt(int num)
    {
        txt_DiamondCount.text = num.ToString();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.CloseGamePanle, Close);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreTxt);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondTxt);
    }
}
