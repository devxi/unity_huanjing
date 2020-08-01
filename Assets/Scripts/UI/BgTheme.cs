using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BgTheme : MonoBehaviour
{
    private void Awake()
    {
        var bg = GetComponent<SpriteRenderer>();
        var bgs = ManagerVars.GetManagerVars().bgThemeSpriteList;
        var randNum = Random.Range(0, bgs.Count);
        bg.sprite = bgs[randNum];
    }
}
