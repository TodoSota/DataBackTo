using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/AttackData")]
public class AttackData : ScriptableObject
{
    public GameObject BulletPrefab;
    public AttackAction ActionLogic;
    public float LifeTime;
    public float Damage;
    public float Speed;
    public PlayerCondition Condition;

    public LayerMask TargetLayers;
}