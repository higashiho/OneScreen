using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Col;
using GameManager;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        // インスタンス化
        public ColComponent EnemyCol{get;private set;}

        void Start()
        {
            EnemyCol = new ColComponent(this.transform.localScale);
            BaseGameObject.Enemys.Add(this);
        }
    }
}

