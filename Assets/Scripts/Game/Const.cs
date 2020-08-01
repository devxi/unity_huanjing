
using UnityEngine;

namespace Game
{
    public class Const : MonoBehaviour
    {
        
    }

    public enum JumpDirection
    {
        /// <summary>
        /// 左跳
        /// </summary>
        Left,
        /// <summary>
        /// 右跳
        /// </summary>
        Right
    }
    
    
    /// <summary>
    /// 生成的障碍物位置
    /// </summary>
    public enum ObstacleDirection
    {
        Left,
        Right
    }

    public enum PlayerState
    {
        /// <summary>
        /// 正在跳跃
        /// </summary>
        Jumping,
        /// <summary>
        /// 限制状态
        /// </summary>
        Idle,
    }

    public enum GameStage 
    {
        /// <summary>
        /// 游戏未开始
        /// </summary>
        None,
        /// <summary>
        /// 游戏进行中
        /// </summary>
        Playing,
        /// <summary>
        /// 游戏结束
        /// </summary>
        Gameover,
    }

}