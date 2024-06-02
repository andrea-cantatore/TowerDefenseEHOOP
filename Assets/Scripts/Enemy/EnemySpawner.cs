using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform m_waveObjective;
    [SerializeField] private float m_spawnRate = 0.3f;
    private int m_waveCount;
    [SerializeField] private int m_waveSize = 10;
    [SerializeField] private int m_waveIncrementation = 5;
    private float m_timer = 0f;
    private float m_waveDelay = 40f;
    private int m_remainingEnemies;
    private Vector3 m_originalSpawnPosition;

    private void Start()
    {
        m_originalSpawnPosition = transform.position;
        WaveStarter();
        m_remainingEnemies = m_waveSize;
    }

    private void OnEnable()
    {
        EventManager.OnEnemyDeath += DecreaseEnemyCount;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDeath -= DecreaseEnemyCount;
    }


    private void Update()
    {
        if (m_timer >= m_waveDelay || m_remainingEnemies == 0)
        {
            m_waveSize += m_waveIncrementation;
            m_remainingEnemies = m_waveSize;
            m_waveCount++;
            m_timer = 0f;
            m_waveDelay += 10f;
            WaveStarter();
        }
        else
        {
            m_timer += Time.deltaTime;
        }
    }

    private void WaveStarter()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < m_waveSize; i++)
        {
            GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject();
            if (enemy.TryGetComponent(out IEnemy enemyComponent) && m_waveObjective.parent.TryGetComponent(
                                                                     out ICore coreComponent)
                                                                 && enemy.TryGetComponent(
                                                                     out NavMeshAgent navMeshAgent))
            {
                enemy.SetActive(true);
                enemy.transform.position = m_originalSpawnPosition;
                navMeshAgent.SetDestination(m_waveObjective.position);
                enemyComponent.NavMeshAgent = navMeshAgent;
                enemyComponent.Core = coreComponent;

                if (enemyComponent.MovementPrediction.TryGetComponent(out NavMeshAgent navmesh))
                {
                    navmesh.SetDestination(m_waveObjective.position);
                }

            }
            yield return new WaitForSeconds(m_spawnRate);
        }
    }

    public void DecreaseEnemyCount()
    {
        m_remainingEnemies--;
    }
}
