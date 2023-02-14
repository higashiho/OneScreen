using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public string CheckHit(Vector3 tmpPos, Vector3 tmpColPos, ColComponent col)
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
                    // どの面が触れているかを返す
                    return upRightDistance(Point, col);
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
                    // どの面が触れているかを返す
                    return downLeftDistance(Point, col);
                }
            }
            return "NoCol";
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
                // 負の数が入らないように絶対値にする
                (Mathf.Abs(Point.y - tmpCol.Point.y), "Up"),
                (Mathf.Abs(Point.x - tmpCol.Point.x), "Right"),
                (Mathf.Abs(Point.x - tmpCol.Point.x), "Left"),
            };

            // 上面座標より右と左の座標が小さい時上面判定
            if(tmpDis[0].dis > tmpDis[1].dis && tmpDis[0].dis > tmpDis[2].dis)
                return tmpDis[0].mask;

            // それ以外の場合
            else 
            {
                var tmpDisX = Point.x - tmpCol.Point.x;
                // x座標の差が負の数の場合右側判定
                if(tmpDisX < 0)
                    return tmpDis[1].mask;
                // x座標の差が正の数の場合は左側判定
                else
                    return tmpDis[2].mask;
            }

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
                // 負の数が入らないように絶対値にする
                (Mathf.Abs(tmpCol.Point.y - Point.y), "Down"),
                (Mathf.Abs(Point.x - tmpCol.Point.x), "Right"),
                (Mathf.Abs(Point.x - tmpCol.Point.x), "Left"),
            };

            // 下面座標より右と左の座標が小さい時下面判定
            if(tmpDis[0].dis > tmpDis[1].dis && tmpDis[0].dis > tmpDis[2].dis)
                return tmpDis[0].mask;

            // それ以外の場合
            else 
            {
                var tmpDisX = Point.x - tmpCol.Point.x;
                // x座標の差が負の数の場合右側判定
                if(tmpDisX < 0)
                    return tmpDis[1].mask;
                // x座標の差が正の数の場合は左側判定
                else
                    return tmpDis[2].mask;
            }
            
        }
    }
}

