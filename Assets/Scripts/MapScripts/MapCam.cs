using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene{
    public class MapCam : MonoBehaviour
    {
        // カメラの位置情報
        private Transform _transform;
        void Start()
        {
            _transform = GetComponent<Transform>();
        }

        // 指定ポジションへの
        public void Move(Vector3 vec)
        {
            // 移動を滑らかにしたい

            _transform.position = vec;
        }
    }
}