using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlatformGroupType
{
    Grass = 0,
    Winter = 1,
}
public class PlatformSpawner : MonoBehaviour
{
    private int SpawnPlatformCount = 5;
    private int SpawnSpikeFakePlatformCount = 5;// 在钉子平台 后面继续生产5个迷惑平台
    
    private ManagerVars vars;

    /// <summary>
    /// 当前正在生成的平台的位置
    /// </summary>
    private Vector2 platformPos;

    private bool isLeftSpawn;
    
    private Sprite selectPlatformSprite;

    private PlatformGroupType groupType;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        platformPos = vars.starPlatformPos;
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }

    void Start()
    {
        RandomPlatformTheme();
        for (int i = 0; i < 5; i++)
        {
            DecidePath();
        }
        
        //生成player
    }

    void DecidePath()
    {
        if (SpawnPlatformCount > 0)
        {
            SpawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            //反向生成
            isLeftSpawn = !isLeftSpawn;
            //随机生成1-3个
            SpawnPlatformCount = Random.Range(1, 4);
            SpawnPlatformCount = 1;
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject platformObject = null;
        //生成单个平台
        if (SpawnPlatformCount >= 1)
        {
            platformObject = SpawnNomalPlatform();
        }
                    
        //每一轮生成的最后一个平台，生成组合平台
        else if (SpawnPlatformCount == 0)
        {
            //生成组合平台 
            int ran = Random.Range(0, 3);
                if (ran == 0)
                {
                    //生成通用组合平台
                    platformObject = SpawnCommonPlatformGroup();
                }
                else if (ran == 1)
                {
                    //生成主题组合平台
                    switch (groupType)
                    {
                        //生成草地主题
                        case PlatformGroupType.Grass:
                        {
                            platformObject = SpawnGrassGroupPlatform();
                            break;
                        }
                        //生成冬季主题
                        case PlatformGroupType.Winter:
                        {
                            platformObject = SpawnWinterGroupPlatform();
                            break;
                        }
                    }
                }
                else if(ran == 2)
                {
                    //生成钉子组合平台 
                    platformObject = SpawnSpikePlatform(); 
                }
            }
        SpawnDiamond(platformObject);
    }

    private void SpawnDiamond(GameObject platformObject)
    {
        //生成平台的时候有一点几率生成钻石在平台上
        var chance = vars.spawnerDiamondChance;
        var canSpawnDiamond = chance > Random.value;
        if (canSpawnDiamond)
        {
            GameObject diamond = Instantiate(vars.diamondPre, platformObject.transform);
            diamond.transform.localPosition = new Vector2(0, 0.57f);
        }
    }

    /// <summary>
    /// 生成普通平台(单个)
    /// </summary>
    GameObject SpawnNomalPlatform()
    {
        GameObject go = Instantiate(vars.normalPlatfrom, GameManager.Instance.PlatformContainer);
        go.transform.position = platformPos;
        SetPlatormPos(go);
        return go;
    }

    GameObject SpawnCommonPlatformGroup()
    {
        GameObject go = Instantiate(vars.normalPlatfrom, GameManager.Instance.PlatformContainer);
        go.transform.position = platformPos;
        SetPlatormPos(go);
        return go;
    }
    
    private void SetPlatormPos(GameObject go)
    {
        go.transform.localPosition = platformPos;
        var x = isLeftSpawn ? platformPos.x - vars.nextXPos : platformPos.x + vars.nextXPos;
        var y = platformPos.y + vars.nextYPos;
        platformPos = new Vector2(x, y);
        var platformScript = go.GetComponent<PlatformScript>();
        platformScript.Init(selectPlatformSprite);
    }

    GameObject SpawnGrassGroupPlatform()
    {
        int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        GameObject go = Instantiate(vars.grassPlatformGroup[ran], GameManager.Instance.PlatformContainer);
        SetPlatormPos(go);
        return go;
    }
    
    GameObject SpawnWinterGroupPlatform()
    {
        int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        GameObject go = Instantiate(vars.winterPlatformGroup[ran], GameManager.Instance.PlatformContainer);
        SetPlatormPos(go);
        return go;
    }

    GameObject SpawnSpikePlatform()
    {
        //钉子生成方向
        var isLeftSpike = !isLeftSpawn;
        var parent = GameManager.Instance.PlatformContainer;
        GameObject go = isLeftSpike ? Instantiate(vars.spikePlatformLeft, parent) : Instantiate(vars.spikePlatformRight, parent);
        SetPlatormPos(go);

        var platformWithSpike = go.transform.Find("PlatformWithSpike");
        //迷惑平台生成
        for (int i = 1; i <= SpawnSpikeFakePlatformCount; i++)
        {
            GameObject fakePlatform = Instantiate(vars.normalPlatfrom, GameManager.Instance.PlatformContainer);
            var x = isLeftSpike ? platformWithSpike.position.x - i * vars.nextXPos : platformWithSpike.position.x + i * vars.nextXPos;
            var y = platformWithSpike.position.y + i * vars.nextYPos;
            fakePlatform.transform.position = new Vector2(x, y);
            fakePlatform.GetComponent<PlatformScript>().Init(selectPlatformSprite);
        }

        return go;
    }
    
    /// <summary>
    /// 生成随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);//0 normal 1 grass 2 ice
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if (ran == 2)
        {    
            //随机到2 定义为 冬季主题
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            //其他 normal 和 grass 都定义为草地主题
            groupType = PlatformGroupType.Grass;
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
}
