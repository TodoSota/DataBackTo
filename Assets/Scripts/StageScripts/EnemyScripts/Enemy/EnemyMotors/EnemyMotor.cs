using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMotor : MonoBehaviour {
    protected Rigidbody rb;
    protected EnemyStatus status;
    protected EnvironmentSensor sensor;
    protected EnemyController controller;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        status = GetComponent<EnemyStatus>();
        sensor = GetComponent<EnvironmentSensor>();
        controller = GetComponent<EnemyController>();
    }
    public abstract void Move(Vector3 dir, float speed);
    public virtual void Stop() => rb.velocity = Vector3.zero;

    public abstract void Patrol();
    public virtual void Attack()
    {
        if (controller.Target is null)
        {
            Debug.Log($"<color=red>{gameObject.name}<color> : Œ©Ž¸‚Á‚½");
            controller.ChangeState(controller.enemyPatrolState);
            return;
        }
        status.MainAttack?.ActionLogic.Execute(controller, status.MainAttack);
    }

    public virtual void DisablePhysics(){
        Stop();
        rb.isKinematic = true;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
    }
    public virtual void FlipX()
    {
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void KnockBack(Vector3 direction)
    { 
        StopCoroutine(nameof(KnockBackSequence));
        StartCoroutine(KnockBackSequence(direction));
    }

    IEnumerator KnockBackSequence(Vector3 direction)
    {
        float kbX = (direction.x == 0f) 
                ? (transform.right.x >= 0 ? -1f : 1f)
                : Mathf.Sign(direction.x);
        float hopY = 0.5f;
        Vector3 kbDir = new Vector3(kbX, hopY, 0f);
        rb.AddForce(kbDir * 5f, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.2f);

    }
}

