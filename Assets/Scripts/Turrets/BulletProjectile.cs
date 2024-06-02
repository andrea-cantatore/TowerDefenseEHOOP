using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletProjectile : MonoBehaviour, IBullet
{
    public Transform Target { get; set; }
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private int m_damage = 2;
    private Rigidbody m_rb;

    private void Update()
    {
        transform.LookAt(Target.position);
        m_rb.velocity = transform.forward * m_speed;
        if(Target.gameObject.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
    }

    public void Launch()
    {
        transform.LookAt(Target);
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = transform.forward * m_speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemy enemy))
        {
            enemy.TakeDamage(m_damage);
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;
        Launch();
    }
}


