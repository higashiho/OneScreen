using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField, Header("Enemy移動スピード")]
        private float enemyMoveSpeed;
        public float EnemyMoveSpeed{get{return enemyMoveSpeed;}}
    }
}

