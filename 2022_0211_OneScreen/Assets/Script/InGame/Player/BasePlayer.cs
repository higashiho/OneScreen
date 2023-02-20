using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Col;
using GameManager;
using DG.Tweening;

namespace Player
{
    /// <summary>
    /// プレイヤー変数用Baseクラス
    /// </summary>
    public class BasePlayer
    {
        // コンストラクタ
        public BasePlayer(BaseGameObject tmp)
        {
            GameObjectManager = tmp;
            Initialization();
        }
        // playerのデータ
        public PlayerData PlayersData{get;private set;} = null;
        // プレイヤーのハンドル
        public AsyncOperationHandle Handle{get;private set;}

        // player
        public GameObject PlayerObj{get;private set;} = null;

        // 挙動開始frame取得用
        protected float? moveStartFlame = null;
        public float? MoveStartFlame{get{return moveStartFlame;} set{moveStartFlame = value;}}
        // 挙動ステート
        protected uint moveState = 0;
        public uint MoveState{get{return moveState;} set{moveState = value;}}
        // 現在の移動スピード
        protected float nowMoveSpeed;
        public float NowMoveSpeed{get{return nowMoveSpeed;} set{nowMoveSpeed = value;}}
        // プレイヤーの現在のHp
        protected float nowHp;
        // 接触判定カウント
        private float touchFlameCount = 0;
        // 初期座標
        protected Vector3 startPos;
        // インスタンス化
        private PlayerMove move;
        public BaseGameObject GameObjectManager{get;private set;}
        
        // 掘る対象エネミー
        private GameObject digEnemy;
        public GameObject DigEnemy{get{return digEnemy;}set{digEnemy = value;}}
        // 当たり判定
        public ColComponent PlayerCol{get;private set;}

        /// <summary>
        /// プレイヤー初期化処理
        /// </summary>
        public async void Initialization()
        {
            move = new PlayerMove(this);

            // プレイヤーデータ取得
            Handle = Addressables.LoadAssetAsync<PlayerData>("Assets/Data/PlayerData.asset");
            await Handle.Task;
            PlayersData = (PlayerData)Handle.Result;
            Addressables.Release(Handle);
            
            // 取得し完了するまで待つ
            Handle = Addressables.LoadAssetAsync<GameObject>("Player");
            await Handle.Task;

            // ゲームオブジェクト型にcastし生成
            var tmpObj = (GameObject)Handle.Result;
            PlayerObj = MonoBehaviour.Instantiate(tmpObj, Vector3.zero, Quaternion.identity);
            
            
            PlayerCol = new ColComponent(PlayerObj.transform.localScale);

            
            // 変数初期化
            nowHp = PlayersData.MaxHp;
            startPos = PlayerObj.transform.position;

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
                if(tmp == null || tmp.EnemyCol == null || !tmp.EnemyObj.activeSelf)
                    break;
                Debug.Log(PlayerCol.CheckHit(
                PlayerObj.transform.position, 
                tmp.EnemyObj.transform.position, 
                tmp.EnemyCol
                ));
                // 自分以外のエネミーとの当たり判定
                switch(PlayerCol.CheckHit(
                PlayerObj.transform.position, 
                tmp.EnemyObj.transform.position, 
                tmp.EnemyCol
                ))
                {
                    case "Up":
                    #if DEBUG
                    #else
                        if(nowHp <= 0)
                        {
                            GameObject.GameStatus = BaseGameObject.GameState.GAME_END;
                            return;
                        }
                    #endif
                        // Hpを減らして初期座標に戻す
                        nowHp--;
                        PlayerObj.transform.position = startPos;
                        GameObjectManager.HpHeartImage[(int)nowHp].sprite = GameObjectManager.HpHeartSprite;
                        break;
                    case "Down":
                        // オブジェクトの上に乗るフラグを立てる
                        touchFlameCount = InGameConst.MAX_TOUCH_COUNT;
                        MoveState |= InGameConst.MOVE_STATE_NO_GRAVITATION;
                        break;
                    case "Right":
                        // 挙動を止める
                        MoveState &= ~InGameConst.MOVE_STATE_RIGHT_MOVE;
                        NowMoveSpeed = 0;

                        // 掘る対象を格納
                        DigEnemy = tmp.EnemyObj;

                        // めり込むのを避けるため左に動かす
                        var tmpPos = PlayerObj.transform.position;
                        tmpPos.x -= InGameConst.PLAYER_MOVE_CHANGE_SPEED;
                        PlayerObj.transform.position = tmpPos;
                        break;
                    case "Left":
                        // 挙動を止める                        
                        MoveState &= ~InGameConst.MOVE_STATE_LEFT_MOVE;
                        NowMoveSpeed = 0;   
                        
                        // めり込むのを避けるため右に動かす
                        tmpPos = PlayerObj.transform.position;
                        tmpPos.x += InGameConst.PLAYER_MOVE_CHANGE_SPEED;
                        PlayerObj.transform.position = tmpPos;   

                        // 掘る対象を格納
                        DigEnemy = tmp.EnemyObj;
                        break;
                    default:
                        // 5フレーム接触していなければオブジェクトの上に乗るフラグを折る
                        if(touchFlameCount <= 0)
                            MoveState &= ~InGameConst.MOVE_STATE_NO_GRAVITATION;
                        
                        else
                            touchFlameCount--;
                        break;
                }
            }
        }
    }
}

