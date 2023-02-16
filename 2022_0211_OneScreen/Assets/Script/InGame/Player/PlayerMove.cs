using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameManager;

namespace Player
{
    /// <summary>
    /// プレイヤー挙動関数管理クラス
    /// </summary>
    public class PlayerMove
    {
        // インスタンス化
        private BasePlayer player;
        public PlayerMove(BasePlayer tmpPlayer)
        {
            player = tmpPlayer;
        }

        /// <summary>
        /// 挙動関数
        /// </summary>
        public void Movements()
        {
            var tmpPos = player.PlayerObj.transform.position;

            // 移動
            switch(player.MoveState)
            {
                case 0:
                    player.MoveStartFlame = null;
                    break;
                case InGameConst.MOVE_STATE_RIGHT_MOVE:
                    rightMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_RIGHT_MOVE | InGameConst.MOVE_STATE_JUMP:
                    rightMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_LEFT_MOVE:
                    leftMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_LEFT_MOVE | InGameConst.MOVE_STATE_JUMP:
                    leftMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_RESET:
                    resetMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_RESET | InGameConst.MOVE_STATE_JUMP:
                    resetMove(ref tmpPos);
                    break;
                case InGameConst.MOVE_STATE_JUMP:
                    break;
                default:
                    break;
            }
            // ジャンプのステートがたっていないとき重力を掛ける
            if(player.MoveState == 0 || (player.MoveState & InGameConst.MOVE_STATE_JUMP) == 0 )
                gravity(ref tmpPos);

            
            // 画面外に出ないように処理
            stopMove(ref tmpPos);
            

            player.PlayerObj.transform.position = tmpPos;

            // 掘る挙動
            digEnemy();
        }

        /// <summary>
        /// 移動停止挙動関数
        /// </summary>
        /// <param name="pos"></param>
        private void resetMove(ref Vector3 pos)
        {
            // フレームカウントが取得できていないとき取得する
            if(player.MoveStartFlame == null)
                player.MoveStartFlame = player.GameObjectManager.FrameCount;
            // プレイヤーのスピードが正の数の場合
            if(player.NowMoveSpeed > 0)
            {
                // スピードを減らして止める
                player.NowMoveSpeed -= Mathf.Pow(player.GameObjectManager.FrameCount - (float)player.MoveStartFlame, 2) / InGameConst.MAX_FLAME;
                // ０以下になったらReset終了
                if(player.NowMoveSpeed <= 0)
                    player.MoveState &= ~InGameConst.MOVE_STATE_RESET;
            }
            
            else
            {
                // スピードを増やして止める
                player.NowMoveSpeed += Mathf.Pow(player.GameObjectManager.FrameCount - (float)player.MoveStartFlame, 2) / InGameConst.MAX_FLAME;
                // ０以上になったらReset終了
                if(player.NowMoveSpeed >= 0)
                    player.MoveState &= ~InGameConst.MOVE_STATE_RESET;
            }
            
            pos.x += player.NowMoveSpeed * Time.deltaTime;
        }
        /// <summary>
        /// 右移動関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void rightMove(ref Vector3 pos)
        {
            // フレームカウントが取得できていないとき取得する
            if(player.MoveStartFlame == null)
                player.MoveStartFlame = player.GameObjectManager.FrameCount;
            
            // 挙動開始フレームと現在のフレーム計算し2乗し1秒で10にするためにフレーム数の２乗割る
            player.NowMoveSpeed += Mathf.Pow(player.GameObjectManager.FrameCount - (float)player.MoveStartFlame, 2) / Mathf.Pow(InGameConst.MAX_FLAME, 2);

            if(player.NowMoveSpeed >= player.PlayersData.PlayerMoveSpeed)
                player.NowMoveSpeed = player.PlayersData.PlayerMoveSpeed;
            
            pos.x += player.NowMoveSpeed * Time.deltaTime;
        }

        /// <summary>
        /// 左移動関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void leftMove(ref Vector3 pos)
        {
            
            // フレームカウントが取得できていないとき取得する
            if(player.MoveStartFlame == null)
                player.MoveStartFlame = player.GameObjectManager.FrameCount;
            
            // 挙動開始フレームと現在のフレーム計算し2乗し1秒で10にするためにフレーム数の２乗割る
            player.NowMoveSpeed -= Mathf.Pow(player.GameObjectManager.FrameCount - (float)player.MoveStartFlame, 2) / Mathf.Pow(InGameConst.MAX_FLAME, 2);

            if(player.NowMoveSpeed <= -player.PlayersData.PlayerMoveSpeed)
                player.NowMoveSpeed = -player.PlayersData.PlayerMoveSpeed;
            
            pos.x += player.NowMoveSpeed * Time.deltaTime;
        }

        /// <summary>
        /// 重力関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void gravity(ref Vector3 pos)
        {
            pos.y -= InGameConst.GRAVITATION * Time.deltaTime / 2;
        }

        /// <summary>
        /// 画面外に出ない処理関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void stopMove(ref Vector3 pos)
        {
            
            // 画面外下
            if(pos.y <= InGameConst.PLAYER_STOP_POS.y)
            {
                pos.y = InGameConst.PLAYER_STOP_POS.y;
            }
            // 画面外右端
            if(pos.x >= InGameConst.PLAYER_STOP_POS.x)
            {
                pos.x = InGameConst.PLAYER_STOP_POS.x;
                return;
            }
            // 画面外左端
            if(pos.x <= -InGameConst.PLAYER_STOP_POS.x)
            {
                pos.x = -InGameConst.PLAYER_STOP_POS.x;
                return;
            }
        }

        /// <summary>
        /// プレイヤーの掘る挙動関数
        /// </summary>
        private async void digEnemy()
        {
            if(player.DigEnemy && (player.MoveState &= InGameConst.MOVE_STATE_DIG) != 0)
            {
                if(player.DigEnemy.activeSelf)
                {
                    // オブジェクトを非表示
                    player.DigEnemy.SetActive(false);
                    
                    // ２秒待つ
                    await UniTask.Delay(InGameConst.GRAVITY_RETUN * InGameConst.CHANGE_SECOND, cancellationToken:player.GameObjectManager.Cts.Token);
                    
                    // Cancel処理
                    if(player.GameObjectManager.Cts.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    // Active中のオブジェクトでMoveFlagのDownがtrueの場合falseに変更
                    for(int i = 0; i < BaseGameObject.Enemys.Count; i++)
                    {
                        var tmpEnemy = BaseGameObject.Enemys[i];
                        if(tmpEnemy.EnemyObj == null)
                            continue;

                        if(tmpEnemy == null)
                            break;
                        if(tmpEnemy.EnemyObj.activeSelf && tmpEnemy.MoveStop[0].moveFlag)
                        {
                            tmpEnemy.MoveStop[0] = (false, "Down");
                            tmpEnemy.MoveStop[1] = (true, "Side");
                        }
                        // 一フレーム待つ
                        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken:player.GameObjectManager.Cts.Token);
                        
                        // Cancel処理
                        if(player.GameObjectManager.Cts.Token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                    player.DigEnemy = null;
                    player.MoveState &= ~InGameConst.MOVE_STATE_DIG;
                }
            }
        }
    } 
}

