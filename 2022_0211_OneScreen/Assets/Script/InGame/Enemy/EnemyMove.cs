using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// エネミー挙動クラス
    /// </summary>
    public class EnemyMove
    {
        // インスタンス化
        private BaseEnemy enemy;
        public EnemyMove(BaseEnemy tmpEnemy)
        {
            enemy = tmpEnemy;
        }

        /// <summary>
        /// エネミー挙動管理関数
        /// </summary>
        public void Movements()
        {
            var tmpPos = enemy.EnemyObj.transform.position;
            // 左右移動のフラグがたっていないとき左右移動させる
            if(!enemy.MoveStop[1].moveFlag)       
                // 左右移動
                move(ref tmpPos);

            // 画面外判定処理
            stopMove(ref tmpPos);
            // 重力処理
            gravity(ref tmpPos);
            enemy.EnemyObj.transform.position = tmpPos;
        }

        /// <summary>
        /// 左右移動関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void move(ref Vector3 pos)
        {
            // 左右に一定間隔で動く
            pos.x += Mathf.Sin(Time.time) * enemy.EnemysData.EnemyMoveSpeed * Time.deltaTime;
        }

        /// <summary>
        /// 重力関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void gravity(ref Vector3 pos)
        {
            // エネミーはゆっくり落とすため重力を1/4にする
            pos.y -= (InGameConst.GRAVITATION / InGameConst.GRAVITATION_DIVISION_ENEMY) * Time.deltaTime;
        }

         /// <summary>
        /// 画面外に出ない処理関数
        /// </summary>
        /// <param name="pos">座標管理Vector</param>
        private void stopMove(ref Vector3 pos)
        {
            
            // 画面外下についたら削除
            if(pos.y <= (InGameConst.STOP_POS.y + enemy.EnemyCol.HalScale.y))
            {
                // 落下を止める
                enemy.MoveStop[0] = (true, "Down");
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

