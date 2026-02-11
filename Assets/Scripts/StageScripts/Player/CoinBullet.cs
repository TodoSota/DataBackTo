using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 2f;

    void Start()
    {
        // ‘O•û•ûŒü‚É‘¬“x‚ğ—^‚¦‚é
        GetComponent<Rigidbody>().velocity = transform.right * speed;

        // lifetime Œã‚É©•ª‚ğÁ‹
        Destroy(gameObject,  lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>()?.TakeDamage(1, transform.position);
            Destroy(gameObject); // “–‚½‚ê‚ÎÁ–Å
        }
    }

    /*
    void Update(){}
    */
}
