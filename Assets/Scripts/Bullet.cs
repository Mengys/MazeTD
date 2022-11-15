using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Target = null;
    private float _speed = 5f;
    private float _damageAmount = 5f;

    private void Start() {

    }

    private void Update() 
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }
        Move();
    }

    private void Move()
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        Vector3 translation = direction * _speed * Time.deltaTime;
        if (Vector3.Distance(Target.transform.position, transform.position) < 0.1f)
        {
            Target.GetComponent<IDamagable>().Damage(_damageAmount);
            Destroy(gameObject);
        }
        transform.Translate(translation);
    }
}
