using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家撞到障碍物时 死亡
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameAudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.Die, PlayerDieType.TriggerObstacle);
        }
    }
}
