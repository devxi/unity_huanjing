using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class MainPanel : MonoBehaviourBase
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;
    private Button btn_Sound;

    private ManagerVars vars;

    // Start is called before the first frame update
    void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    void Init()
    {
        btn_Start = GetUI<Button>("btn_Start");
        btn_Shop = GetUI<Button>("Btns/btn_Shop");
        btn_Rank = GetUI<Button>("Btns/btn_Rank");
        btn_Sound = GetUI<Button>("Btns/btn_Sound");
    
        btn_Start.onClick.AddListener(OnStartButtonClick);
        btn_Shop.onClick.AddListener(OnShopButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Sound.onClick.AddListener(OnSoundtButtonClick);
        
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);
    }

    private void OnStartButtonClick()
    {
        Debug.Log("点击了开始");
        Hide();
        Instantiate(vars.GamePanel, transform.parent);
    }
    
    private void OnShopButtonClick()
    {
        Debug.Log("点击了商店");
    }
    private void OnRankButtonClick()
    {
        Debug.Log("点击了排行");
    }
    private void OnSoundtButtonClick()
    {
        Debug.Log("点击了声音设置");
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
