using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoBehaviourBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    
    protected T GetUI<T>(string path)
    {
        Transform t = transform.Find(path);
        if (!t)
        {
            throw  new Exception("找不到名称为" + path +"的ui");
        }

        T btn = t.GetComponent<T>();
        if (btn == null)
        {
            Debug.LogWarning("找不到名称为" + path +"的ui");
        }
        return btn;
    }
}
