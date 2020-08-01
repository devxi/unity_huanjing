using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家撞到障碍物时 死亡
        if (other.gameObject.CompareTag("Player"))
        {
            EventCenter.Broadcast(EventDefine.Die, PlayerDieType.TriggerObstacle);
        }
    }
}
