using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene{
    public class MapCam : MonoBehaviour
    {
        public void Move(Vector3 vec)
        {
            transform.position = vec;
        }
    }
}