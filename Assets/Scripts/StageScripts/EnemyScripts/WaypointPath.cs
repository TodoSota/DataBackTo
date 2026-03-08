using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public bool usePath = false;
    public Vector3 pointA = new Vector3(-1, 0, 0);
    public Vector3 pointB = new Vector3(1, 0, 0);

    private Vector3 _basePosition;

    private void Awake()
    {
        _basePosition = transform.position;
    }
    public Vector3 GetWorldA()
    {
        // 再生中は固定された基準点から、エディタ中は現在の本体位置から計算する
        Vector3 basePos = Application.isPlaying ? _basePosition : transform.position;
        return basePos + pointA;
    }
    public Vector3 GetWorldB()
    {
        Vector3 basePos = Application.isPlaying ? _basePosition : transform.position;
        return basePos + pointB;
    }
    private void OnDrawGizmos()
    {
        if (!usePath) return;

        Gizmos.color = Color.cyan;

        Vector3 center = Application.isPlaying ? _basePosition : transform.position;

        Gizmos.DrawLine(center + pointA, center + pointB);
        Gizmos.DrawWireSphere(center + pointA, 0.1f);
        Gizmos.DrawWireSphere(center + pointB, 0.1f);
    }
}