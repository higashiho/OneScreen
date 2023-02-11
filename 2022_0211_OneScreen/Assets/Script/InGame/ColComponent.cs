using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Col
{
    /// <summary>
    /// 当たり判定クラス
    /// </summary>
    public class ColComponent
    {
        // オブジェクト半分サイズ
        public Vector3 HalScale{get;private set;}
        // オブジェクトの座標
        public Vector3 Point{get;private set;}


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tmpPos">座標</param>
        public ColComponent(Vector3 tmpScale)
        {
            HalScale = tmpScale / 2;

        }

        /// <summary>
        /// 上下の当たり判定
        /// </summary>
        /// <param name="tmpPos">当たり判定判断側のpos</param>
        /// <param name="tmpColPos">当たり判定判断される側のpos</param>
        /// <param name="col">当たり判定Component</param>
        /// <returns></returns>
        public bool CheckHit(Vector3 tmpPos, Vector3 tmpColPos, ColComponent col)
        {
            // 座標更新 + 当たり判定の範囲追加
            Point = tmpPos; col.Point = tmpColPos;

            // 上面の判定
            if((Point.y + HalScale.y) - (col.Point.y - col.HalScale.y) >= 0 && 
                Point.y - col.Point.y <= 0)
            {
                // 左右とも当たり判定が取れていたらtrue
                if((Point.x + HalScale.x) - (col.Point.x - col.HalScale.x) >= 0 && 
                    Point.x - col.Point.x <= 0 || 
                    (Point.x - HalScale.x) - (col.Point.x + col.HalScale.x) <= 0 && 
                    col.Point.x - Point.x <= 0)
                {
                    // 上と右どちらが一番触れているか
                    Debug.Log(upRightDistance(Point, col));

                    return true;
                }
                
            }
            // 下面の判定
            if((Point.y - HalScale.y) - (col.Point.y + col.HalScale.y) <= 0 && 
                col.Point.y - Point.y <= 0)
            {
                // 左右とも当たり判定が取れていたらtrue
                if((Point.x + HalScale.x) - (col.Point.x - col.HalScale.x) >= 0 && 
                    Point.x - col.Point.x <= 0 || 
                    (Point.x - HalScale.x) - (col.Point.x + col.HalScale.x) <= 0 && 
                    col.Point.x - Point.x <= 0)
                {   
                    // 左としたどちらが一番触れているか
                    Debug.Log(downLeftDistance(Point, col));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 上か右のどちらが一番触れているか判断関数
        /// </summary>
        /// <param name="tmpPoint"></param>
        /// <param name="tmpCol"></param>
        private string upRightDistance(Vector3 tmpPoint, ColComponent tmpCol)
        {
            var tmpDis = new List<(float dis, string mask)>
            {
                ((Point.y + HalScale.y) - (tmpCol.Point.y - tmpCol.HalScale.y), "ue"),
                ((Point.x + HalScale.x) - (tmpCol.Point.x - tmpCol.HalScale.x), "migi"),
            };

            var maxElement = tmpDis.OrderByDescending(x => x.dis).FirstOrDefault();

            return maxElement.mask;
        }
        /// <summary>
        /// 下か左のどちらが一番触れているか判断関数
        /// </summary>
        /// <param name="tmpPoint"></param>
        /// <param name="tmpCol"></param>
        private string downLeftDistance(Vector3 tmpPoint, ColComponent tmpCol)
        {
            var tmpDis = new List<(float dis, string mask)>
            {
                ((Point.y - HalScale.y) - (tmpCol.Point.y + tmpCol.HalScale.y), "sita"),
                ((Point.x - HalScale.x) - (tmpCol.Point.x + tmpCol.HalScale.x), "hidari"),
            };

            var maxElement = tmpDis.OrderBy(x => x.dis).FirstOrDefault();

            return maxElement.mask;
        }
    }
}

