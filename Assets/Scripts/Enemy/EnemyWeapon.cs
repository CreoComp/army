using UnityEngine;

public class EnemyWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float _damage;
    public float GetDamageAmount()
    {
       return _damage;
    }
}
