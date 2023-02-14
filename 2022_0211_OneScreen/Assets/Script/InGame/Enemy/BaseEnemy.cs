using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Col;
using GameManager;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy
{
    /// <summary>
    /// エネミー管理クラス
    /// </summary>
    public class BaseEnemy
    {
        // 親オブジェクト格納用
        private GameObject parent;
        // コンストラクタ
        public BaseEnemy(GameObject tmpObj)
        {
            parent = tmpObj;
            Initialization();
        }
        // インスタンス化
        public ColComponent EnemyCol{get;private set;} = null;
        private EnemyMove move = null;

        // エネミーのハンドル
        public AsyncOperationHandle Handle{get;private set;}
        // プレイヤーオブジェクト
        public GameObject EnemyObj{get;private set;} = null;

        // エネミーのデータ
        public EnemyData EnemysData{get; private set;} = null;

        // 挙動出来るか
        public List<(bool moveFlag, string moveName)> MoveStop = new List<(bool moveFlag, string moveName)>
        {
            (false, "Down"),
            (false, "Side"),
        };

        /// <summary>
        /// 初期化処理
        /// </summary>
        public async void Initialization()
        {
            move = new EnemyMove(this);
            // エネミーデータ取得
            Handle = Addressables.LoadAssetAsync<EnemyData>("Assets/Data/EnemyData.asset");
            await Handle.Task;
            EnemysData = (EnemyData)Handle.Result;
            Addressables.Release(Handle);

            // 取得し完了するまで待つ
            Handle = Addressables.LoadAssetAsync<GameObject>("Enemy");
            await Handle.Task;

            // ゲームオブジェクト型にcastし生成
            var tmpObj = (GameObject)Handle.Result;
            EnemyObj = MonoBehaviour.Instantiate(tmpObj, Vector3.zero, Quaternion.identity, parent.transform);
            
            // 座標変更
            PosSet();
            
            // ハンドル開放
            Addressables.Release(Handle);

            // 当たり判定クラス作成
            EnemyCol = new ColComponent(EnemyObj.transform.localScale);
        }

        /// <summary>
        /// 座標指定関数
        /// </summary>
        public void PosSet()
        {
            // 座標変更
            var tmpPos = EnemyObj.transform.position;
            tmpPos.x = UnityEngine.Random.Range(InGameConst.ENEMY_STOP_POS.x, -InGameConst.ENEMY_STOP_POS.x);
            tmpPos.y = InGameConst.ENEMY_POS_Y;
            EnemyObj.transform.position = tmpPos;
        }
        /// <summary>
        /// Enemy更新関数
        /// </summary>
        public void EnemyUpdate()
        {
            // 当たり判定が生成されていて挙動ストップフラグがたっていないとき
            if(EnemyCol != null && !MoveStop[0].moveFlag)
            {
                move.Movements();

                // 当たり判定
                foreach(var tmp in BaseGameObject.Enemys)
                {
                    // オブジェクトにnullが入ってしまった場合抜ける
                    if(tmp == null)
                        break;
                    
                    // オブジェクトが地震と同じ場合処理を飛ばす
                    if(tmp.EnemyObj == this.EnemyObj || tmp.EnemyCol == null)
                        continue;
                    
                    var tmpColName = EnemyCol.CheckHit(
                    EnemyObj.transform.position, 
                    tmp.EnemyObj.transform.position, 
                    tmp.EnemyCol);
                    // 自分以外のエネミーとの当たり判定
                    switch(tmpColName)
                    {
                        case "Up":
                            break;
                        case "Down":
                            MoveStop[0] = (true, "Down");
                            break;
                        case "Right":
                            MoveStop[1] = (true, "Side");
                            break;
                        case "Left":
                            MoveStop[1] = (true, "Side");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}

