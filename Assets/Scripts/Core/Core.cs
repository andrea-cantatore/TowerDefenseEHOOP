using System;
using UnityEngine;

public class Core : MonoBehaviour, ICore
{
    [SerializeField] private int m_hp = 10;
    [SerializeField] private float m_regenRate = 3f;
    [SerializeField] private float m_regenDelay = 5f; //Longer delay if u take dmg
    [SerializeField] private int m_regenAmount = 1;
    private float m_timer = 0f;
    private float m_delayTimer = 0f;

    private void Update()
    {
        if(m_hp < 10)
        {
            Regen();
        }
    }

    public void TakeDamage(int damage)
    {
        m_hp -= damage;
        if (m_hp <= 0)
        {
            Die();
        }
        m_delayTimer = 0f;
    }
    
    private void Regen()
    {
        if (m_delayTimer >= m_regenDelay)
        {
            if(m_timer >= m_regenRate)
            {
                m_hp += m_regenAmount;
                m_timer = 0f;
                return;
            }
            m_timer += Time.deltaTime;
            return;
        }
        m_delayTimer += Time.deltaTime;
    }
    
    private void Die()
    {
        EventManager.OnGameEnd?.Invoke(false);
        Destroy(gameObject);
    }
}

