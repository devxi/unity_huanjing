using System;
using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform target;//摄像机跟随的目标
        private Vector3 offset;//摄像机和跟随的目标之前的偏移
        private Vector2 velocity;//摄像机跟随速度
        private Vector3 originPos = new Vector3(0, 0 ,-10);

        private void Awake()
        {
            originPos = transform.position;
            EventCenter.AddListener(EventDefine.OnGameStart, Reset);
        }

        private void Update()
        {
            if (!target && GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                offset = target.position - transform.position;
            }
        }

        private void FixedUpdate()
        {
            if (target)
            {
                if (target.position.y > transform.position.y)
                {
                    // transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
                    float x = Mathf.SmoothDamp(transform.position.x, target.position.x - offset.x, ref velocity.x,
                        0.05f);
                    float y = Mathf.SmoothDamp(transform.position.y, target.position.y - offset.y, ref velocity.y,
                        0.05f);
                    transform.position = new Vector3(x, y, transform.position.z);
                }
            }
        }

        private void Reset()
        {
            transform.position = originPos;
        }

        private void OnDestroy()
        {
            EventCenter.RemoveListener(EventDefine.OnGameStart, Reset);
        }
    }
}