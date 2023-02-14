using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

            // 左右移動
            move(ref tmpPos);
            // 重力を掛ける
            gravity(ref tmpPos);
            // 画面外に出ないように処理
            stopMove(ref tmpPos);
            

            player.PlayerObj.transform.position = tmpPos;
        }

        /// <summary>
        /// 左右移動関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void move(ref Vector3 pos)
        {
            if(Input.GetMouseButtonDown(0))
            {
                DOTween.Kill(player.PlayerObj.transform);
                var tmpMousPos = Input.mousePosition;
                Vector3 target = Camera.main.ScreenToWorldPoint(tmpMousPos);
                player.PlayerObj.transform.DOMoveX(
                    target.x, player.PlayersData.PlayerMoveSpeed).
                    SetEase(Ease.Linear);
            }
        }
        
        /// <summary>
        /// 重力関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void gravity(ref Vector3 pos)
        {
            pos.y -= InGameConst.GRAVITATION * Time.deltaTime;
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
    } 
}

