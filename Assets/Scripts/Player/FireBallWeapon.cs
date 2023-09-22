using UnityEngine;

public class FireBallWeapon : MonoBehaviour, IWeapon
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
