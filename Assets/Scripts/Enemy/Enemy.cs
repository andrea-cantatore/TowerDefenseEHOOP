using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(Transform))]
public class Enemy : MonoBehaviour, IEnemy
{


    public Transform CorePosition { get; set; }
    public NavMeshAgent NavMeshAgent { get; set; }
    [field: SerializeField] public GameObject MovementPrediction { get; set; }
    public ICore Core { get; set; }
    
    [SerializeField] private int m_hp = 10;
    [SerializeField] private float m_slowDuration = 2f;
    [SerializeField] private float m_slowFactor = 1.5f;
    [SerializeField] private int m_coreDmg = 1;
    [SerializeField] private Vector3 m_originalPosition;
    [SerializeField] private int _coinValue = 5;
    
    private void Update()
    {
        if (Vector3.Distance(transform.position, NavMeshAgent.destination) <= 1f)
        {
            DmgCore();
        }
    }

    private void OnEnable()
    {
        m_hp = 10;
        m_originalPosition = transform.position;
    }


    public void TakeDamage(int damage)
    {
        m_hp -= damage;
        if (m_hp <= 0)
        {
            Die();
        }
    }
    public void SlowEffect()
    {
        NavMeshAgent.speed -= m_slowFactor;
        StartCoroutine(ResetSpeed());
    }
    public void Die()
    {
        EventManager.OnEnemyDeath?.Invoke();
        EventManager.ChangeCoins?.Invoke(_coinValue);
        transform.position = m_originalPosition;
        gameObject.SetActive(false);
    }
    public void DmgCore()
    {
        Core.TakeDamage(m_coreDmg);
        Die();
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(m_slowDuration);
        NavMeshAgent.speed += m_slowFactor;
    }
}
