using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    AttackData Data { get; }
    Vector3 MoveDirection { get; }

    void SetUp(AttackData data);
}
