using UnityEngine;

public class ElectricBall : MonoBehaviour, IWeapon
{
    private float _damage;
    public void Construct(float damage)
    {
        _damage = damage;
    }

    public float GetDamageAmount()
    {
        return _damage;
    }
}
