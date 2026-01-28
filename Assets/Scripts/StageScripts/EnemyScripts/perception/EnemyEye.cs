using UnityEngine;

[System.Serializable]
public class EnemyEye
{
    // 視力
    public float ViewRadius = 10f;
    [Range(0, 360)] public float ViewAngle = 90f;

    [Range(-180, 180)]
    [Tooltip("0なら真横、正なら下向き、負なら上向きに視線が傾きます")]
    public float EyeOffsetAngle = 45f;

    // プレイヤーのレイヤー
    public LayerMask TargetMask;
    // 壁などの遮蔽物のレイヤー（現状グラウンド）
    public LayerMask ObstacleMask;

    private Transform _owner;

    public void Init(Transform owner) => _owner = owner;

    private Vector3 GazeDirection 
    {
        get {
            // 正面方向（right）を Z軸（forward）を軸にしてオフセット分回転させる
            // 2.5Dなので、Z軸回転が「上下の首振り」になります
            return Quaternion.AngleAxis(EyeOffsetAngle, _owner.forward) * _owner.right;
        }
    }

    public Transform CheckForPlayer()
    {
        // 1. 距離の判定（円形の範囲内にターゲットがいるか）
        Collider[] targetsInRadius = Physics.OverlapSphere(_owner.position, ViewRadius, TargetMask);

        foreach (var targetCollider in targetsInRadius)
        {
            Transform target = targetCollider.transform;
            Vector3 dirToTarget = (target.position - _owner.position).normalized;

            // 2. 角度の判定（自分の正面から見て視野角内か）
            if (Vector3.Angle(GazeDirection, dirToTarget) < ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(_owner.position, target.position);

                // 3. 遮蔽物の判定（Rayを飛ばして壁に当たらないか）
                if (!Physics.Raycast(_owner.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    return target; // プレイヤーを発見！
                }
            }
        }
        return null;
    }

    public void DrawViewGizmos()
    {
        if (_owner == null) return;

        Vector3 gaze = GazeDirection;

        // 1. 視界距離（半径）を円で表示
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_owner.position, ViewRadius);

        // 2. 視野角（扇形）の端を表示
        Gizmos.color = Color.yellow;
        
        // 現在の正面（right）を基準に、左右の境界線を計算
        // キャラクターが Y軸回転（0, 180）で動いている想定で、Y軸周りに回転させます
        Vector3 leftBoundary = Quaternion.AngleAxis(-ViewAngle / 2f, _owner.forward) * gaze;
        Vector3 rightBoundary = Quaternion.AngleAxis(ViewAngle / 2f, _owner.forward) * gaze;

        Gizmos.DrawRay(_owner.position, leftBoundary * ViewRadius);
        Gizmos.DrawRay(_owner.position, rightBoundary * ViewRadius);
        Gizmos.color = (CheckForPlayer() != null) ? Color.red : Color.cyan;
    }
}