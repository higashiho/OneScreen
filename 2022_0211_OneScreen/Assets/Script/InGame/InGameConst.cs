using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameConst : MonoBehaviour
{
    // システム用定数
    /// <summary>
    ///  重力定数
    /// </summary>
    public const float GRAVITATION = 9.8f;
    /// <summary>
    /// 座標最小値
    /// </summary>
    public static readonly Vector3 PLAYER_STOP_POS = new Vector3(12.5f, -9.5f, 0); 
    public static readonly Vector3 ENEMY_STOP_POS = new Vector3(12.0f ,-9.3f, 0.0f);
    
    // エネミー用定数
    /// <summary>
    /// エネミー生成速度
    /// </summary>
    public const int ENEMY_SPONE_SPEED = 2;
    /// <summary>
    /// エネミー生成初期ｙ座標
    /// </summary>
    public const float ENEMY_POS_Y = 9.5f;
    /// <summary>
    /// エネミーの重力scaleを割り算する用
    /// </summary>
    public const float GRAVITATION_DIVISION_ENEMY = 4;
    
    // タスク用定数
    /// <summary>
    /// ミリ秒を秒に変換
    /// </summary>
    public const int CHANGE_SECOND = 1000;
}
