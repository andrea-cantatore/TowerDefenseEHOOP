using UnityEngine;

public class BuffedRange : MonoBehaviour, ITurret, IPowerUp
{

    public bool IsPlaced { get; set; }
    
    public int cost { get; set; } = 40;
    
    public int slot { get; set; }
    
    public void Fire()
    {
    }
    public void CheckTarget()
    {
    }
    public int ReturnTurret()
    {
        return 3;
    }
    public int ReturnPowerUp()
    {
        return 0;
    }
}
