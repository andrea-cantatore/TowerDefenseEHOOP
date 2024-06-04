public interface ITurret
{
    public bool IsPlaced { get; set; }
    public int cost { get; set; }
    public int slot { get; set; }
    
    public void Fire();
    public void CheckTarget();
    public int ReturnTurret(); // Added method to return the turret type, 0 is shoot, 1 is freeze, 2 is rocket
    
}
