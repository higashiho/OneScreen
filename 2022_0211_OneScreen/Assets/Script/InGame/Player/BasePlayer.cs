using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace player
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
        // ステージアドレスのハンドル
        public AsyncOperationHandle Handle{get;private set;}

        // player
        public GameObject PlayerObj{get;private set;} = null;

        // インスタンス化
        public PlayerMove Move;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public async void Initialization()
        {
            Move = new PlayerMove(this);
            // 取得し完了するまで待つ
            Handle = Addressables.LoadAssetAsync<GameObject>("Player");
            await Handle.Task;

            // ゲームオブジェクト型にcastし生成
            var tmpObj = (GameObject)Handle.Result;
            PlayerObj = MonoBehaviour.Instantiate(tmpObj, Vector3.zero, Quaternion.identity);

            // メモリ開放
            Addressables.Release(Handle);
        }

        public void PlayerUpdate()
        {
            Move.Movements();
        }
    }
}

