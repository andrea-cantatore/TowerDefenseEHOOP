using System;
using UnityEngine;

public class BuffedFireRate : MonoBehaviour, ITurret, IPowerUp
{

    public bool IsPlaced { get; set; }
    public int cost { get; set; } = 50;
    
    public int slot { get; set; }
    

    public void Fire()
    {
        
    }
    public void CheckTarget()
    {
        
    }
    public int ReturnTurret()
    {
        return 4;
    }
    public int ReturnPowerUp()
    {
        return 1;
    }
}
