using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Col;
using GameManager;

namespace Player
{
    /// <summary>
    /// プレイヤー変数用Baseクラス
    /// </summary>
    public class BasePlayer
    {
        // コンストラクタ
        public BasePlayer()
        {
            Initialization();
        }
        // プレイヤーのハンドル
        public AsyncOperationHandle Handle{get;private set;}

        // player
        public GameObject PlayerObj{get;private set;} = null;

        // インスタンス化
        private PlayerMove move;

        // 当たり判定
        public ColComponent PlayerCol{get;private set;}

        /// <summary>
        /// プレイヤー初期化処理
        /// </summary>
        public async void Initialization()
        {
            move = new PlayerMove(this);
            // 取得し完了するまで待つ
            Handle = Addressables.LoadAssetAsync<GameObject>("Player");
            await Handle.Task;

            // ゲームオブジェクト型にcastし生成
            var tmpObj = (GameObject)Handle.Result;
            PlayerObj = MonoBehaviour.Instantiate(tmpObj, Vector3.zero, Quaternion.identity);
            
            
            PlayerCol = new ColComponent(PlayerObj.transform.localScale);
            // メモリ開放
            Addressables.Release(Handle);
        }

        /// <summary>
        /// プレイヤー更新関数
        /// </summary>
        public void PlayerUpdate()
        {
            move.Movements();

            foreach(var tmp in BaseGameObject.Enemys)
            {
                // エネミーの生成が終わってい無かったら処理終了
                if(tmp == null || tmp.EnemyCol == null)
                    break;

                // エネミーとの当たり判定
                PlayerCol.CheckHit(
                    PlayerObj.transform.position, 
                    tmp.EnemyObj.transform.position, 
                    tmp.EnemyCol
                    );
            }
        }
    }
}

