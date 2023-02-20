using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using Player;
using Enemy;
using Col;

namespace GameManager
{
    public class BaseGameObject : MonoBehaviour
    {
        // インスタンス化
        public static BasePlayer Player{get;private set;}
        public static List<BaseEnemy> Enemys{get;private set;} = new List<BaseEnemy>(100);

        // タスク処理しているか判断用変数
        private UniTask? enemySponTask = null;

        [SerializeField, Header("エネミーの親オブジェクト")]
        private GameObject enemysParent;
        [SerializeField,Header("PlayerのHpイメージ配列")]
        private Image[] hpHeartImage = new Image[3];
        public Image[] HpHeartImage{get{return hpHeartImage;}}
        [SerializeField, Header("PlayerのHpイメージの枠")]
        private Sprite hpHeartSprite;
        public Sprite HpHeartSprite{get{return hpHeartSprite;}}
        [SerializeField, Header("汚染ゲージスライダー")]
        private Slider waterPollutionSlider;
        public Slider WaterPollutionSlider{get{return waterPollutionSlider;}}

        // タスクキャンセル用
        public CancellationTokenSource Cts = new CancellationTokenSource();
        // ゲーム挙動ステート
        public enum GameState
        {
            LOAD,
            MOVE_GAME,
            GAME_END
        }
        public GameState GameStatus = GameState.LOAD;

        // フレームカウント
        public int FrameCount{get;private set;} = 0;

        /// <summary>
        /// インゲーム初期取得関数
        /// </summary>
        protected async void startAcquisition()
        {
            Player = new BasePlayer(this);

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
                        // 生成してリストから削除
                        tmpEnemy = tmp;
                        Enemys.Remove(tmp);
                        // 座標変更
                        tmpEnemy.PosSet();
                        // フラグ初期化
                        tmpEnemy.MoveStop[0] = (false, "Down");
                        tmpEnemy.MoveStop[1] = (false, "Side");
                        tmpEnemy.WaterPollutionSliderValue = false;
                        break;
                    }
                }
            }
            // オブジェクト取得されていない場合
            if(tmpEnemy == null)
            {
                // 生成
                tmpEnemy = new BaseEnemy(enemysParent, this);
            }

            // リストの最後に格納
            Enemys.Add(tmpEnemy);

            // Enemyがインスタンス化されるまで待つ  
            await UniTask.WaitWhile(() => tmpEnemy.EnemyObj == null,cancellationToken:Cts.Token);

            // Cancel処理
            if(Cts.Token.IsCancellationRequested)
            {
                return;
            }
            // スケール変更
            tmpEnemy.ScaleSet();
            // 当たり判定クラス作成
            tmpEnemy.EnemyCol = new ColComponent(tmpEnemy.EnemyObj.transform.localScale);

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
            FrameCount++;
            switch(GameStatus)
            {
                case GameState.LOAD:
                    return;
                case GameState.MOVE_GAME:
                    if(WaterPollutionSlider.value >= WaterPollutionSlider.maxValue)
                        GameStatus = GameState.GAME_END;
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

        // button処理関数================================================
        // ジャンプステートを立てる処理
        public void PlayerSwim()
        {
            // ジャンプステートがたってなければ立てる
            if(Player.MoveState == 0 || (Player.MoveState & InGameConst.MOVE_STATE_JUMP) == 0)
            {
                // フレームカウントをnullにする
                Player.MoveStartFlame = null;
                Player.MoveState &= ~InGameConst.MOVE_STATE_NO_GRAVITATION;
                Player.MoveState |= InGameConst.MOVE_STATE_JUMP;
                
            }
        }
        // 左横移動ステートを立てる処理
        public void PlayerLeftMove()
        {

            // 左横移動ステートがたっていなければ立てる
            if(Player.MoveState == 0 || (Player.MoveState & InGameConst.MOVE_STATE_LEFT_MOVE) == 0)
            {
                // フレームカウントをnullにする
                Player.MoveStartFlame = null;
                // ジャンプステートと何かに乗っているステート以外を折る
                Player.MoveState &= (InGameConst.MOVE_STATE_JUMP | InGameConst.MOVE_STATE_NO_GRAVITATION);
                // 左移動ステートを立てる
                Player.MoveState |= InGameConst.MOVE_STATE_LEFT_MOVE;       
            
                // 掘る対象オブジェクトをnullにする
                Player.DigEnemy = null;
            }
        }
        // 右横移動ステートを立てる処理
        public void PlayerRightMove()
        {
            
            
            // 左横移動ステートがたっていなければ立てる
            if(Player.MoveState == 0 || (Player.MoveState & InGameConst.MOVE_STATE_RIGHT_MOVE) == 0)
            {
                // フレームカウントをnullにする
                Player.MoveStartFlame = null;
                // ジャンプステートと何かに乗っているステート以外を折る
                Player.MoveState &= (InGameConst.MOVE_STATE_JUMP | InGameConst.MOVE_STATE_NO_GRAVITATION);
                // 右移動ステートを立てる
                Player.MoveState |= InGameConst.MOVE_STATE_RIGHT_MOVE;
                // 掘る対象オブジェクトをnullにする
                Player.DigEnemy = null;
            }
        }
        // 移動ボタンを離したときの処理
        public void ResetMove()
        {
            // フレームカウントをnullにする
            Player.MoveStartFlame = null;
            // ジャンプステートと何かに乗っているステート以外をを全て折り止まる挙動を開始させる
            Player.MoveState &= (InGameConst.MOVE_STATE_JUMP | InGameConst.MOVE_STATE_NO_GRAVITATION);
            Player.MoveState |= InGameConst.MOVE_STATE_RESET;
        }
        // スイムボタンを離したときの処理
        public void ResetSwim()
        {
            // フレームカウントをnullにする
            Player.MoveStartFlame = null;

            // ステートを折る
            Player.MoveState &= ~InGameConst.MOVE_STATE_JUMP;
            Player.MoveState |= InGameConst.MOVE_STATE_RESET_SWIM;

        }
        // Digボタン処理関数
        public void DigMove()
        {
            Player.MoveState |= InGameConst.MOVE_STATE_DIG;
        }
        // button処理関数================================================
        
    }
}

