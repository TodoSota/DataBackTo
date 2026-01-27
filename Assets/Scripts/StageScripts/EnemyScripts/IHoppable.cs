using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoppable
{
    float HopAngle{ get; }
    Vector3 _hopDir { get; }
    float HopPower{ get; }
}
