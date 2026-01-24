using UnityEngine;
using System;

namespace MapScene
{
    public class MapInputHandler : MonoBehaviour
    {
        public event Action<Vector2Int> OnMoveInput;
        public event Action OnConfirmInput;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) OnMoveInput?.Invoke(Vector2Int.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) OnMoveInput?.Invoke(Vector2Int.right);
            else if (Input.GetKeyDown(KeyCode.UpArrow)) OnMoveInput?.Invoke(Vector2Int.up);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) OnMoveInput?.Invoke(Vector2Int.down);

            if (Input.GetKeyDown(KeyCode.Return)) OnConfirmInput?.Invoke();
        }
    }
}