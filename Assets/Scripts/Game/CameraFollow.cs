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
            if (target == null && null != GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                offset = target.position - transform.position;
            }
        }

        private void FixedUpdate()
        {

            if (target != null)
            {
                float posX = Mathf.SmoothDamp(transform.position.x,
                    target.position.x - offset.x, ref velocity.x, 0.05f);
                float posY = Mathf.SmoothDamp(transform.position.y,
                   target.position.y - offset.y, ref velocity.y, 0.05f);

                if (posY > transform.position.y)
                    transform.position = new Vector3(posX, posY, transform.position.z);
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