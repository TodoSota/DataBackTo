using UnityEngine;
using System;

namespace MapScene
{
    public class MapInputHandler : MonoBehaviour
    {
        // デリゲートを受ける
        public event Action<Vector2Int> OnMoveInput;
        public event Action OnConfirmInput;

        void Update()
        {
            // 移動キーが押されたときに、委譲された移動用関数を使用（あれば）
            if (Input.GetKeyDown(KeyCode.LeftArrow)) OnMoveInput?.Invoke(Vector2Int.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) OnMoveInput?.Invoke(Vector2Int.right);
            else if (Input.GetKeyDown(KeyCode.UpArrow)) OnMoveInput?.Invoke(Vector2Int.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) OnMoveInput?.Invoke(Vector2Int.down);

            // エンターキーが押されたときに、委譲された決定行為を利用（あれば）
            if (Input.GetKeyDown(KeyCode.Return)) OnConfirmInput?.Invoke();
        }
    }
}