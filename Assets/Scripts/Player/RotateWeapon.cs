using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    private List<GameObject> Weapons = new List<GameObject>();

    [SerializeField] private GameObject[] _weapons;

    [SerializeField] private float radius = 1f;
    [SerializeField] private float speedRotation = 3f;
    [SerializeField] private float offsetYCenter = 1f;
    private float angle;
    private Vector3 center;
    private float circleLenght;

    private Health Health;

    private int _indexWeapon;
    private int _weaponsCount;


    private void Start()
    {
        Health = GetComponent<Health>();
        circleLenght = radius * Mathf.PI * 2;
        _indexWeapon = SaveLoadService.Instance.PlayerData.WeaponSelectedIndex;
        _weaponsCount = SaveLoadService.Instance.PlayerData.SwordLevel;
        AddWeapon(SaveLoadService.Instance.PlayerData.SwordLevel);
    }

    private void OnEnable()
    {
        SaveLoadService.Instance.ChangeCharacteristic += ChangeWeapon;
    }

    private void OnDisable()
    {
        SaveLoadService.Instance.ChangeCharacteristic -= ChangeWeapon;
    }

    private void ChangeWeapon()
    {
        if(_indexWeapon != SaveLoadService.Instance.PlayerData.WeaponSelectedIndex)
        {
            int count = Weapons.Count;
            for(int i = 0; i < count; i++)
            {
                Debug.Log("count");
                var weapon = Weapons[0];
                Weapons.Remove(weapon);
                Destroy(weapon);

            }

            _indexWeapon = SaveLoadService.Instance.PlayerData.WeaponSelectedIndex;
            AddWeapon(SaveLoadService.Instance.PlayerData.SwordLevel);

        }

        if(_weaponsCount != SaveLoadService.Instance.PlayerData.SwordLevel)
        {
            _weaponsCount = SaveLoadService.Instance.PlayerData.SwordLevel;
            AddWeapon(1);
        }
    }


    private void AddWeapon(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject weaponParent = new GameObject();
            var weapon = Instantiate(_weapons[_indexWeapon]);
            weapon.transform.SetParent(weaponParent.transform);
            weapon.transform.rotation = Quaternion.Euler(0, -90, 0);
            int damage = SaveLoadService.Instance.PlayerData.DamageLevel * 2;
            weapon.AddComponent<Weapon>().Construct(Health, damage);

            var collider = weapon.AddComponent<MeshCollider>();
            collider.convex = true;
            collider.isTrigger = true;
            collider.sharedMesh = weapon.GetComponent<MeshFilter>().sharedMesh;


            weaponParent.name = "Weapon " + weapon.name;

            Weapons.Add(weaponParent);

        }
    }


    private void LateUpdate()
    {
        center = new Vector3(transform.position.x, transform.position.y + offsetYCenter, transform.position.z);

        CheckInput();
        Rotate();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddWeapon(1);
        }
    }

    private void Rotate()
    {
        angle += Time.deltaTime * speedRotation;

        for(int i = 0; i < Weapons.Count; i++)
        {
            float x;
            float z;
            x = radius * Mathf.Cos(angle + (circleLenght / Weapons.Count) * i) + center.x;
            z = radius * Mathf.Sin(angle + (circleLenght / Weapons.Count) * i) + center.z;

            Weapons[i].transform.localPosition = new Vector3(x, center.y, z);
        }
    }
}
