using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


            // 重力を掛ける
            gravity(ref tmpPos);
            // 画面外に出ないように処理
            stopMove(ref tmpPos);

            player.PlayerObj.transform.position = tmpPos;
        }
        
        /// <summary>
        /// 重力関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void gravity(ref Vector3 pos)
        {
            pos.y -= InGameConst.Gravitation * Time.deltaTime;
        }

        /// <summary>
        /// 画面外に出ない処理関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void stopMove(ref Vector3 pos)
        {
            
            // 画面外下
            if(pos.y <= InGameConst.STOP_POS.y)
            {
                pos.y = InGameConst.STOP_POS.y;
            }
            // 画面外右端
            if(pos.x >= InGameConst.STOP_POS.x)
            {
                pos.x = InGameConst.STOP_POS.x;
                return;
            }
            // 画面外左端
            if(pos.x <= -InGameConst.STOP_POS.x)
            {
                pos.x = -InGameConst.STOP_POS.x;
                return;
            }
        }
    } 
}

