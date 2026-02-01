using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemySelfDamagedOnTouch : MonoBehaviour
{
    private EnemyController _controller;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.collider;
        var status = _controller.Status;
        if (status == null || status.MainAttack == null) return;
        int targetMask = status.MainAttack.TargetLayers.value;
        int otherLayer = 1 << collision.gameObject.layer;
        if ((targetMask & (1 << other.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                // <追加>接触でダメージ受けるんだっけ？責任の所在が気になる
                _controller.ChangeState(_controller.enemyDamagedState);
            }
        }
    }
}