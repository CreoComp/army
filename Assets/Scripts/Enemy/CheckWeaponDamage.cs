using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWeaponDamage : MonoBehaviour
{
    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IWeapon weapon))
        {
            _health.GetDamage(weapon.GetDamageAmount());
        }

    }
}
