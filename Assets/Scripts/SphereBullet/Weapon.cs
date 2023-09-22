using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    private Health _attackerHealth;
    private Transform _parent;
    private float _damage;

    public void Construct(Health health, float damage)
    {
        _attackerHealth = health;
        _parent = transform.parent;
        _damage = damage;
    }


    private void Update()
    {
        Vector3 _player = new Vector3(_attackerHealth.transform.position.x, _parent.position.y, _attackerHealth.transform.position.z);
        _parent.up = (_parent.position - _player).normalized;
        _parent.rotation = Quaternion.Euler(90, _parent.eulerAngles.y, _parent.eulerAngles.z);
    }

    public float GetDamageAmount()
    {
        return _damage;
    }

}
