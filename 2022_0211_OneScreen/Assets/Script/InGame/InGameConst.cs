using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameConst : MonoBehaviour
{
    // システム用定数
    /// <summary>
    /// フレーム最大値
    /// </summary>
    public const int MAX_FLAME = 60;
    /// <summary>
    ///  重力定数
    /// </summary>
    public const float GRAVITATION = 9.8f;
    /// <summary>
    /// 座標最小値
    /// </summary>
    public static readonly Vector3 PLAYER_STOP_POS = new Vector3(12.5f, -6.7f, 0); 
    public static readonly Vector3 ENEMY_STOP_POS = new Vector3(12.0f ,-7.2f, 0.0f);

    // プレイヤー用定数
    /// <summary>
    /// プレイヤーの重力scaleを減らす用
    /// </summary>
    public const float GRAVITATION_DIVISION_PLAYER = 2f;
    /// <summary>
    /// 右移動ステート
    /// </summary>
    public const uint MOVE_STATE_RIGHT_MOVE = 0x01;
    /// <summary>
    /// 左移動ステート
    /// </summary>
    public const uint MOVE_STATE_LEFT_MOVE = 0x02;
    /// <summary>
    /// ジャンプ移動ステート
    /// </summary>
    public const uint MOVE_STATE_JUMP = 0x04;
    /// <summary>
    /// 挙動リセットステート
    /// </summary>
    public const uint MOVE_STATE_RESET = 0x08;
    /// <summary>
    /// 掘る挙動ステート
    /// </summary>
    public const uint MOVE_STATE_DIG = 0x10;
    /// <summary>
    /// 重力を掛けないようにする
    /// </summary>
    public const uint MOVE_STATE_NO_GRAVITATION = 0x20;
    /// <summary>
    /// 泳ぎリセット挙動ステート
    /// </summary>
    public const uint MOVE_STATE_RESET_SWIM = 0x40;
    /// <summary>
    /// 接触判定カウント最大値
    /// </summary>
    public const uint MAX_TOUCH_COUNT = 20;
    /// <summary>
    /// y座標最大値
    /// </summary>
    public const float MAX_POS_Y = 7.0f;
    /// <summary>
    /// プレイヤーの座標が変わる値の変化スピード
    /// </summary>
    public const float PLAYER_MOVE_CHANGE_SPEED = 0.1f;

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
    /// エネミーの重力scaleを減らす用
    /// </summary>
    public const float GRAVITATION_DIVISION_ENEMY = 3;
    /// <summary>
    /// 地面が消されて落下を再開する時間
    /// </summary>
    public const int GRAVITY_RETUN = 2;
    /// <summary>
    /// エネミースケール最大値
    /// </summary>
    public const float MAX_ENEMY_SCALE = 2.0f;
    /// <summary>
    /// エネミースケール最小値
    /// </summary>
    public const float MIN_ENEMY_SCALE = 0.5f;
    
    // タスク用定数
    /// <summary>
    /// ミリ秒を秒に変換
    /// </summary>
    public const int CHANGE_SECOND = 1000;
}
