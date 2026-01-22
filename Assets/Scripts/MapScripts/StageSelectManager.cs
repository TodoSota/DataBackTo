using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene
{
    public class StageSelectManager : MonoBehaviour
    {
        public StageNode[] StageNodes;

        private int CurrentStageIndex;

        void Start()
        {
            CurrentStageIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Return))
            {

            }

        }
    }
}