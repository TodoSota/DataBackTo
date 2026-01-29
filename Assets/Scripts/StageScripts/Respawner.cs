using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 3.0f;
    private Timer _timer = new Timer();

    private void Update()
    {
        _timer.Update(Time.deltaTime);
        if(_timer.IsFinished){
            DisappearRespawner();
            return;
        }
    }
    public void SetRespawner(Vector3 position)
    {
        SearchSafePlace(position);
        gameObject.SetActive(true);
        _timer.Start(_lifeTime);
    }

    public void DisappearRespawner()
    {
        gameObject.SetActive(false);
    }

    private void SearchSafePlace(Vector3 position)
    {
        
    }
}