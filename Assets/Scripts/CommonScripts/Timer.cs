using UnityEngine;

[System.Serializable]
public class Timer
{
    private float _limitTime;
    private float _currentTime;
    public bool IsRunning { get; private set; }

    public bool IsFinished => _currentTime >= _limitTime;

    public float Progress => Mathf.Clamp01(_currentTime / _limitTime);

    // タイマーを開始・リセットする
    public void Start(float limitTime)
    {
        _limitTime = limitTime;
        _currentTime = 0f;
        IsRunning = true;
    }

    // 毎フレームの更新処理（deltaTimeを渡す）
    public void Update(float deltaTime)
    {
        if (!IsRunning || IsFinished) return;
        _currentTime += deltaTime;
    }

    public void Stop() => IsRunning = false;
}