using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField, Header("player移動スピード")]
        private float playerMoveSpeed;
        public float PlayerMoveSpeed{get{return playerMoveSpeed;}}
    }
}

