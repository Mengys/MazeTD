using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ActionOnTimer _actionOnTimer;

    private float _radius = 3f;
    private bool _readyToShoot = true;

    private void Update()
    {
        //не оптимально
        var target = GetTarget(GetMobsInRadius());
        if (target != null)
        {
            float angle = Vector3.SignedAngle(Vector3.right, target.transform.position - transform.position, Vector3.forward);
            transform.RotateAround(transform.position, Vector3.forward, angle - transform.rotation.eulerAngles.z);
        }
        
        if (target != null && _readyToShoot)
        {
            Shoot(target);
            _readyToShoot = false;
            _actionOnTimer.SetTimer(1f, () => _readyToShoot = true);
        }
    }

    private void SpawnBullet()
    {
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
    }

    private GameObject GetTarget(List<GameObject> mobs)
    {
        GameObject target = null;
        float minDistance = float.MaxValue;
        foreach (GameObject mob in mobs)
        {
            if (mob.GetComponent<Mob>().DistanceToBase < minDistance)
            {
                target = mob;
                minDistance = mob.GetComponent<Mob>().DistanceToBase;
            }
        }
        return target;
    }

    private List<GameObject> GetMobsInRadius()
    {
        var mobsInRadius = new List<GameObject>();
        foreach (GameObject mob in GameController.Instance.Mobs)
        {
            if (Vector3.SqrMagnitude(mob.transform.position - transform.position) < _radius * _radius)
            {
                mobsInRadius.Add(mob);
            }
        }
        return mobsInRadius;
    }

    private void Shoot(GameObject target)
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Target = target;
    }

    private void RotateToTarget()
    {

    }
}
