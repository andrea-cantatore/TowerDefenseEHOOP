using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class FreezeTurret : MonoBehaviour, ITurret
{
    [SerializeField] private Transform m_shootSpawnPoint;
    [SerializeField] private float m_fireRate = 1f;
    [SerializeField] private float m_range = 20f;
    private float m_timer = 0f;
    private bool m_canFire = true;
    private Transform m_target;
    public bool IsPlaced { get; set; }
    private void Update()
    {
        if(!IsPlaced)
            return;
        CheckTarget();
        if (m_canFire)
        {
            Fire();
        }
        if (m_canFire)
        {
            float originalZRotation = transform.rotation.eulerAngles.z;
            transform.LookAt(new Vector3(m_target.position.x, transform.position.y, m_target.position.z));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = originalZRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    public void Fire()
    {
        if (m_timer >= m_fireRate)
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetBulletFreezePooledObject();
            if (bullet.TryGetComponent(out IBullet freezeProjectile))
            {
                m_timer = 0f;
                bullet.transform.position = m_shootSpawnPoint.position;
                bullet.transform.rotation = m_shootSpawnPoint.rotation;
                freezeProjectile.SetTarget(m_target);
                bullet.SetActive(true);
            }
        }
        else
        {
            m_timer += Time.deltaTime;
        }
    }

    public void CheckTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_range);

        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IEnemy enemy))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }
        if (closestCollider != null)
        {
            m_target = closestCollider.transform;
            m_canFire = true;
        }
        else
        {
            m_canFire = false;
            m_target = null;
        }
    }
    
    public int ReturnTurret()
    {
        return 1;
    }
}
