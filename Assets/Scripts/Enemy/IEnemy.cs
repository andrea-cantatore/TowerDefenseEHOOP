using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
     public void TakeDamage(int damage);
     public void SlowEffect();
     public void Die();
     public void DmgCore();
     
     public ICore Core { get; set; }
     public Transform CorePosition { get; set; }
     
     public NavMeshAgent NavMeshAgent { get; set; }
     
     public GameObject MovementPrediction { get; set; }
}

