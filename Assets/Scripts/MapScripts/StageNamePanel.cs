using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MapScene
{
    public class StageNamePanel : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetStageName(string stageName)
        {
            _text.text = stageName;
        }
    }
}