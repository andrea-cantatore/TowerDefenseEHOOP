using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class RocketProjectile : MonoBehaviour, IBullet
{
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_launchAngle = 45f;
    [SerializeField] private float m_explosionRadius = 5f;
    public Vector3 Target;
    
    

    
    public void SetTarget(Transform target)
    {
        Target = target.position;
        Launch();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, Target) <= 0.5f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IEnemy enemy))
                {
                    enemy.TakeDamage(5);
                }
            }
            gameObject.SetActive(false);
        }
    }

    public void Launch()
    {
        Vector3 direction = Target - transform.position;
        float yOffset = direction.y;
        direction.y = 0;

        float distance = direction.magnitude;

        float angle = m_launchAngle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angle);
        distance += yOffset / Mathf.Tan(angle);
        
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle));
        Vector3 velocityVector = velocity * direction.normalized;

        m_rb.velocity = velocityVector;
    }
    

}
