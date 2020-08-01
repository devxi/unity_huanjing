using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;


public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] SpriteRenderers;
    
    public GameObject obstacle;//障碍物

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    
    private float fallingDownTime = 1.0f;

    private bool isNeedFallingDown;

    private bool isWalkaCross = false; //该块平台是否已经被走过了 
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Init(Sprite sprite)
    {
        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].sprite = sprite;
        }
        
        if (obstacle != null)
        {
            int randNum = UnityEngine.Random.Range(0, 2);
            if (randNum == 0)
            {
                //障碍物在左
                obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x ,obstacle.transform.localPosition.y,obstacle.transform.localPosition.z);
            }
            else if (randNum == 1)
            {
                //障碍物在右
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x ,obstacle.transform.localPosition.y,obstacle.transform.localPosition.z );
            }
        }
    }

    private void Update()
    {
        if (isNeedFallingDown)
        {
            fallingDownTime -= Time.deltaTime;
            if (fallingDownTime <= 0)
            {
                //被踩过的平台 指定时间后 自动坠落 移除碰撞组件
                isWalkaCross = true;
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        //当玩家跳到平台上时触发
        if (other.gameObject.CompareTag("Player"))
        {
            //平台 指定时间后 坠落
            isNeedFallingDown = true;
        }
    }
}
