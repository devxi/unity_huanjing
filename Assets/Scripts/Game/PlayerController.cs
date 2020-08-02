

//#define DEBUG 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using DG.Tweening;
using Game;
using UnityEngine;



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

    public AudioSource audioSource;
    public Transform RayDown, RayLeft, RayRight;
    public LayerMask platformLayer, obstacleLayer;


    private ManagerVars vars;
    private Vector2 curPlatformPos = Vector2.zero; //当前玩家所在平台位置
    private PlayerState playerState = PlayerState.Idle;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider2d;
    private bool isDie = false;


    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        //audioSource = 
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
        if (GameManager.Instance.CurGameStage != GameStage.Playing)
        {
            return;
        }

        if (isDie)
        {
            return;
        }

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
        Debug.DrawRay(RayDown.position, Vector3.down, Color.red);
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
        GameManager.Instance.GameAudioSource.PlayOneShot(vars.jumpClip);
        playerState = PlayerState.Jumping;
        
        if (jumpDirection == JumpDirection.Left)
        {
            var duration = 0.2f;
            var targetPos = new Vector2(transform.position.x - vars.jumpXPos,transform.position.y + vars.jumpYPos);
            transform.DOMove(targetPos, duration).OnComplete(OnJumpFinished);
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (jumpDirection == JumpDirection.Right)
        {
            var duration = 0.2f;
            var targetPos = new Vector2(transform.position.x + vars.jumpXPos,transform.position.y + vars.jumpYPos);
            transform.DOMove(targetPos, duration).OnComplete(OnJumpFinished); ;
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

        //跳完后， 射线检测 脚底下有没有 平台 没有就挂了

        //只检测  platformLayer 层
        RaycastHit2D hit = Physics2D.Raycast(RayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider && hit.collider.gameObject.CompareTag("Platform"))
        {
            //有平台
            EventCenter.Broadcast(EventDefine.AddScore);
            curPlatformPos = hit.collider.transform.position;
            #if DEBUG
            hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            #endif
        }
        else
        {
            if (hit.collider == null)
            {
                Debug.Log("没有平台, 射线检测不collider");
            }
            else
            {
                Debug.Log("没有平台, 射线检测到的物体tag :" + hit.collider.tag + ", g tag" + hit.collider.gameObject.tag);
            }
            //没平台
            //Time.timeScale = 0;
            Die(PlayerDieType.FallingDie);
        }
    }

    void Die(PlayerDieType dieType)
    {
        isDie = true;
        if (dieType== PlayerDieType.TriggerObstacle)
        {
            Debug.Log("玩家跌落，游戏结束");
            GameManager.Instance.GameAudioSource.PlayOneShot(vars.fallClip);
            //播放死亡特效
            GameObject dieEffect = Instantiate(vars.deathEffect);
            dieEffect.transform.position = transform.position;
            //玩家跌落死亡时 让玩家层级比平台低
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
        Invoke("ShowGameOverPanel", 2.0f);
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

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家吃到钻石
        if (other.gameObject.CompareTag("Pickup_Diamond"))
        {
            GameManager.Instance.GameAudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<PlayerDieType>(EventDefine.Die, Die);
    }
}
