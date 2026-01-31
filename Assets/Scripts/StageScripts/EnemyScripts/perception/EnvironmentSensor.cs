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

    public bool IsGrounded()
    {
        Vector3 checkPos = transform.position + new Vector3(0, -col.bounds.extents.y, 0);

        float width = col.bounds.extents.x * 0.8f;
        Vector3 halfExtents = new Vector3(width, 0.05f, 0.1f);

        return Physics.CheckBox(checkPos, halfExtents, transform.rotation, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider>();
        Bounds bounds = col.bounds;
        Gizmos.color = Color.green;
        
        Vector3 checkPos = transform.position + new Vector3(0, -bounds.extents.y, 0);

        float width = bounds.extents.x * 0.8f; // X
        float thickness = 0.05f;              // Y
        float depth = 0.1f;                  // Z
        
        Vector3 halfExtents = new Vector3(width, thickness, depth); 

        Matrix4x4 previousMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(checkPos, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
        Gizmos.matrix = previousMatrix; 
    }
}