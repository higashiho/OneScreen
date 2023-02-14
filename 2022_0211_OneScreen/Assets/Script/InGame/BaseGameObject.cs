using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Player;
using Enemy;

namespace GameManager
{
    public class BaseGameObject : MonoBehaviour
    {
        // インスタンス化
        public static BasePlayer Player;
        public static List<BaseEnemy> Enemys = new List<BaseEnemy>(100);

        // タスク処理しているか判断用変数
        private UniTask? enemySponTask = null;

        [SerializeField, Header("フレームレートカウント")]
        private float frameCount;
        public float FrameCount{get{return frameCount;} set{frameCount = value;}}

        [SerializeField, Header("エネミーの親オブジェクト")]
        private GameObject enemysParent;
        // ゲーム挙動ステート
        public enum GameState
        {
            LOAD,
            MOVE_GAME,
            GAME_END
        }
        public GameState GameStatus = GameState.LOAD;

        /// <summary>
        /// インゲーム初期取得関数
        /// </summary>
        protected async void startAcquisition()
        {
            Player = new BasePlayer();

            // playerがインスタンス化されるまで待つ
            await UniTask.WaitWhile(() => Player.PlayerObj == null);

            // ロードがすべて終わったらステート更新
            GameStatus = GameState.MOVE_GAME;
        }

        /// <summary>
        /// エネミー生成処理
        /// </summary>
        protected async UniTask enemySpone()
        {

            // n秒に一回生成
            await UniTask.Delay(InGameConst.ENEMY_SPONE_SPEED * InGameConst.CHANGE_SECOND);

            BaseEnemy tmpEnemy = null;
            if(Enemys.Count > 0)
            {
                foreach(var tmp in Enemys)
                {
                    // tmpにnullが入ってしまった場合処理終了
                    if(tmp == null)
                        break;
                    
                    // 非表示オブジェクトを取得
                    if(!tmp.EnemyObj.gameObject.activeSelf)
                    {
                        tmpEnemy = tmp;
                        // 座標変更
                        tmpEnemy.PosSet();
                        // フラグ初期化
                        tmpEnemy.MoveStop[0] = (false, "Down");
                        tmpEnemy.MoveStop[1] = (false, "Side");
                        break;
                    }
                }
            }
            // オブジェクト取得されていない場合
            if(tmpEnemy == null)
            {
                // 生成
                Enemys.Add(new BaseEnemy(enemysParent));
                
                // エネミーリストの最後の要素のオブジェクト取得
                tmpEnemy = Enemys[Enemys.Count - 1];
            }

            // Enemyがインスタンス化されるまで待つ  
            await UniTask.WaitWhile(() => tmpEnemy.EnemyObj == null);

            // 非表示の場合表示
            if(!tmpEnemy.EnemyObj.activeSelf)
                tmpEnemy.EnemyObj.SetActive(true);

            // 生成が終わったためタスクを空にする
            enemySponTask = null;
        }

        /// <summary>
        /// ゲーム更新関数
        /// </summary>
        protected void gameUpdate()
        {
            switch(GameStatus)
            {
                case GameState.LOAD:
                    return;
                case GameState.MOVE_GAME:
                    Player.PlayerUpdate();
                    if(enemySponTask == null)
                    {
                        enemySponTask = enemySpone();
                    }
                    if(Enemys.Count > 0)
                    {
                        foreach(var tmp in Enemys)
                        {
                            // nullが入ってきた場合は処理終了
                            if(tmp == null)
                                break;
                            
                            tmp.EnemyUpdate();
                        }
                    }
                    break;
                case GameState.GAME_END:
                    return;
                default:
                    break;
            }
        }
    }
}

