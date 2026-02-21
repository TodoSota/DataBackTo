using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SparkBullet : MonoBehaviour
{
    [SerializeField] private int _sparkAmount = 5;
    private float _bulletRadius;
    [SerializeField] float _offset;
    private float _radius;

    [SerializeField] private VisualEffect _sparkEffect;
    
    private List<VisualEffect> _sparks = new List<VisualEffect>();

    [SerializeField] private Vector2 _waitTimeRange = new Vector2(0.1f, 0.5f); 

    private Timer _timer = new Timer();
    void Start()
    {
        _radius = GetComponent<CapsuleCollider>().radius * _offset;
        
        for (int i = 0; i < _sparkAmount; i++)
        {
            VisualEffect spark = Instantiate(_sparkEffect, transform.position, Quaternion.identity);
            spark.transform.SetParent(transform);
            _sparks.Add(spark);
        }

        ResetTimer();
    }

    void Update()
    {
        _timer.Update(Time.deltaTime);
        
        if(_timer.IsFinished) {
            PlaySpark();
            ResetTimer();
        }
    }

    void ResetTimer()
    {   
        float waitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
        _timer.Start(waitTime);
    }

    void PlaySpark()
    {
        List<VisualEffect> idleSparks = new List<VisualEffect>();

        foreach (VisualEffect vfx in _sparks)
        {
            if (vfx.aliveParticleCount <= 0) idleSparks.Add(vfx);
        }

        if (idleSparks.Count > 0)
        {
            int randomIndex = Random.Range(0, idleSparks.Count);
            var spark = idleSparks[randomIndex];

            spark.Reinit();
            SetPosition(spark);
            spark.SendEvent("OnFire");
        }
    }

    void SetPosition(VisualEffect _spark)
    {
        // 開始位置を決める
        Vector3 from = Random.onUnitSphere * _radius;

        // 回転方向を決める
        Quaternion rot = Random.rotation;
        Quaternion halfRot = Quaternion.Slerp(Quaternion.identity, rot, 0.5f);
        
        // 中央と終点を計算
        Vector3 controlPoint = halfRot * from;
        Vector3 to = rot * from;

        _spark.SetVector3("From", from);
        _spark.SetVector3("ControlPoint", controlPoint);
        _spark.SetVector3("To", to);
    }
}
