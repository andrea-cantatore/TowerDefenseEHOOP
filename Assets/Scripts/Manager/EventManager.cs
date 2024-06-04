using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<bool> OnGameEnd;
    public static Action OnEnemyDeath;
    public static Action<int> ChangeCoins;
    public static Action<bool> OnGamePause;
}
