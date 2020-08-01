using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerDieType 
{
    /// <summary>
    /// 跌死
    /// </summary>
    FallingDie,
    /// <summary>
    /// 触发障碍物死亡
    /// </summary>
    TriggerObstacle
}

public class PlayerController : MonoBehaviour
{

    private ManagerVars vars;
    private Vector2 curPlatformPos = Vector2.zero; //当前玩家所在平台位置
    private PlayerState playerState = PlayerState.Idle;
    private Rigidbody2D rigidbody2d;
    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider2d;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        rigidbody2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        EventCenter.AddListener<PlayerDieType>(EventDefine.Die, Die);
    }

    void Start()
    {
        
    }

    void Update()
    {
        var touchPos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            if (touchPos.y <= Screen.height / 2.0f)
            {
                Jump();
            }
            else
            {
                Debug.LogWarning("点击屏幕的下半部分才能触发跳跃");
            }

        }
    }


    private JumpDirection jumpDirection
    {
        get
        {
            var touchPos = Input.mousePosition;
            
            return touchPos.x <= Screen.width / 2 ? JumpDirection.Left : JumpDirection.Right;
        }

    }

    private void Jump()
    {
        if (playerState.Equals(PlayerState.Jumping))
        {
            //正在跳跃过程不能再跳了
            return;
        }

        playerState = PlayerState.Jumping;
        
        if (jumpDirection == JumpDirection.Left)
        {
            var duration = 0.2f;
            var targetPos = new Vector2(transform.position.x - vars.jumpXPos,transform.position.y + vars.jumpYPos);
            transform.DOMove(targetPos, duration);
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (jumpDirection == JumpDirection.Right)
        {
            var duration = 0.2f;
            var targetPos = new Vector2(transform.position.x + vars.jumpXPos,transform.position.y + vars.jumpYPos);
            transform.DOMove(targetPos, duration);
            transform.localScale = new Vector3(1,1,1);
        }
        
    }

    private void OnJumpFinished()
    {
        if (playerState.Equals(PlayerState.Idle))
        {    
            //重复调用 只有 jumping -> idle 才算有效调用
            return;
        }
        playerState = PlayerState.Idle;
        //继续生成平台
        EventCenter.Broadcast(EventDefine.DecidePath);
        EventCenter.Broadcast(EventDefine.AddScore);
    }

    void Die(PlayerDieType dieType)
    {
        Debug.Log("玩家死亡，游戏结束");
        if (dieType== PlayerDieType.TriggerObstacle)
        {
            //播放死亡特效
            GameObject dieEffect = Instantiate(vars.deathEffect);
            dieEffect.transform.position = transform.position;
        }
        Invoke("ShowGameOverPanel", 2.0f);
        gameObject.SetActive(false);
    }
    

    void ShowGameOverPanel()
    {
        EventCenter.Broadcast(EventDefine.CloseGamePanle);
        //销毁玩家
        Destroy(gameObject);
    }


    private Vector2 NextPlatformLeft
    {
        get => new Vector2(curPlatformPos.x - vars.nextXPos, curPlatformPos.y + vars.nextYPos);
    }
    
    private Vector2 NextPlatformRight
    {
        get => new Vector2(curPlatformPos.x + vars.nextXPos, curPlatformPos.y + vars.nextYPos);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            //玩家和平台发送碰撞 才算一次跳跃完毕
            curPlatformPos = other.transform.position;
            OnJumpFinished();
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup_Diamond"))
        {
            EventCenter.Broadcast(EventDefine.AddDiamond);
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<PlayerDieType>(EventDefine.Die, Die);
    }
}
