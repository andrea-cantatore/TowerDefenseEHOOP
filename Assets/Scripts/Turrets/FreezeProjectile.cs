using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FreezeProjectile : MonoBehaviour, IBullet
{
    public Transform Target { get; set; }
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private int m_damage = 1;
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
            enemy.SlowEffect();
            enemy.TakeDamage(m_damage);
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(Transform target)
    {
        Debug.Log(target.name);
        Target = target;
        Launch();
    }
}
