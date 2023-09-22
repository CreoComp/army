using System.Collections.Generic;

public class PlayerData
{
    public int DamageLevel = 1;
    public int HealthLevel = 1;
    public int SwordLevel = 1;
    public int NowLevel = 1;

    public int Money;

    public int WeaponSelectedIndex;
    public List<int> BoughtWeapons = new List<int>()
    {
        0
    };

    public bool isUseLazer;
    public bool isUseFireBall;
}
