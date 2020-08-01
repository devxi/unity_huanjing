using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    
    public GameObject Player;
    
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    
    // public List<>
    
    public GameObject normalPlatfrom;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    
    public Vector2 starPlatformPos = new Vector2(0,-2.4f);

    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public float jumpYPos = 1.0f, jumpXPos = 0.54f;

    /// <summary>
    /// 钻石生成几率
    /// </summary>
    [Range(0,1)]
    public float spawnerDiamondChance = 10 / 100.0f;
    
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject deathEffect;
    public GameObject diamondPre;
    public AudioClip jumpClip, fallClip, hitClip, diamondClip, buttonClip;
    public Sprite musicOn, musicOff;
    
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }

}
