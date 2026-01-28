using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAction : ScriptableObject
{
    public abstract void Execute(EnemyControllerAbstract owner, AttackData data);
}