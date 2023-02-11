using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GameObjectController : BaseGameObject
    {
        // Start is called before the first frame update
        void Start()
        {
            // 初期化処理が終わるまで待つ
            startAcquisition();
        }

        // Update is called once per frame
        void Update()
        {
            gameUpdate();

        }
    }
}

