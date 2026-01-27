using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSensor : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float ledgeOffset = 0.1f;
    private Collider col;
    private float _modelRadius;
    private Vector3 _bottomOffset;

    private void Awake()
    {
        col = GetComponent<Collider>();
        CalculateSensorBounds();
    }
    // 崖かどうかを判定する機能
    public bool IsAtLedge()
    {
        float distanceToEdge = col.bounds.extents.x;
        Vector3 checkPos = transform.position + (transform.right * (distanceToEdge + ledgeOffset));

        float rayLength = 2.0f; 

        // 下向きに地面があるか？レイキャストで調べる
        Debug.DrawRay(checkPos + Vector3.up * 0.1f, Vector3.down * rayLength, Color.red);

        // Raycast の第3引数も rayLength に合わせて伸ばす
        return !Physics.Raycast(checkPos + Vector3.up * 0.1f, Vector3.down, rayLength, groundLayer);
    }

    // 壁かどうかを判定する機能
    public bool IsHittingWall()
    {
        float distanceToEdge = col.bounds.extents.x;
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.right, distanceToEdge + 0.2f, groundLayer);
    }

    private void CalculateSensorBounds()
    {
        Bounds bounds = col.bounds;
        _modelRadius = bounds.extents.x * 0.8f;
        _bottomOffset = new Vector3(0, -bounds.extents.y, 0);
    }

    public bool IsGrounded()
    {
        Vector3 checkPos = transform.position + _bottomOffset;
        return Physics.CheckSphere(checkPos, _modelRadius, groundLayer);
        
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider>();
        Bounds bounds = col.bounds;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -bounds.extents.y, 0), bounds.extents.x * 0.8f);
    }
}