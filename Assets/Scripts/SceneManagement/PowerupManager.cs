using UnityEngine;

public class PowerupManager : Singleton<PowerupManager>
{
    public bool WallJumpUnlocked { get; set; } = false;
    public bool DashUnlocked { get; set; } = false;
}
