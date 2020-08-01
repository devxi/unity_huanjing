using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //检测平台掉落，掉落到这里的平台，可以销毁了
        if (other.gameObject.CompareTag("Platform"))
        {
            Destroy(other.gameObject);
        } 
        else if(other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.CurGameStage == GameStage.Playing)
            {            //销毁玩家
                EventCenter.Broadcast(EventDefine.Die, PlayerDieType.FallingDie);
            }
        }
    }
}
