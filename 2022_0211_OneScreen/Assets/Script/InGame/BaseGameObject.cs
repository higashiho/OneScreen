using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using player;

namespace GameManager
{
    public class BaseGameObject : MonoBehaviour
    {
        // インスタンス化
        public static BasePlayer Player;


        // ゲーム挙動ステート
        public enum GameState
        {
            LOAD,
            MOVE_GAME,
            GAME_END
        }
        public GameState GameStatus = GameState.LOAD;

        /// <summary>
        /// 初期取得関数
        /// </summary>
        protected async void startAcquisition()
        {
            Player = new BasePlayer();

            // playerがインスタンス化されるまで待つ
            await UniTask.WaitWhile(() => Player.PlayerObj == null);

            // ロードがすべて終わったらステート更新
            GameStatus = GameState.MOVE_GAME;
        }

        protected void gameUpdate()
        {
            switch(GameStatus)
            {
                case GameState.LOAD:
                    return;
                case GameState.MOVE_GAME:
                    Player.PlayerUpdate();
                    break;
                case GameState.GAME_END:
                    return;
                default:
                    break;
            }
        }
    }
}

