using System;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textSelected;

    [SerializeField] private GameObject button;
    [SerializeField] private TextMeshProUGUI textButton;

    private bool _isBought;
    private int _index;
    private int _cost;

    private ButtonBuyWeaponSkin[] swords;

    private void Start()
    {
        swords = transform.GetComponentsInChildren<ButtonBuyWeaponSkin>();
        foreach (int item in SaveLoadService.Instance.PlayerData.BoughtWeapons)
        {
            swords[item].Unlock();
        }

        foreach(var sword in swords)
        {
            sword.Construct();
        }
        swords[SaveLoadService.Instance.PlayerData.WeaponSelectedIndex].ChoseWeapon();

    }

    public void SelectWeapon(int index, int cost)
    {
        _isBought = false;
        foreach(int boughtSwordIndex in SaveLoadService.Instance.PlayerData.BoughtWeapons)
        {
            if(boughtSwordIndex == index)
            {
                _isBought = true;
                break;
            }
        }
        
        if(_isBought)
        {
            if (SaveLoadService.Instance.PlayerData.WeaponSelectedIndex == index)
            {
                TextSelected();
            }
            else
            {
                ButtonConfirm();
            }
        }
        else
        {
            _cost = cost;
            ButtonBuy();
        }
        swords[_index].UnSelect();
        swords[index].Select();
        swords[SaveLoadService.Instance.PlayerData.WeaponSelectedIndex].ChoseWeapon();
        
        _index = index;

    }

    public void ActionButton()
    {
        if(_isBought)
        {
            swords[SaveLoadService.Instance.PlayerData.WeaponSelectedIndex].UnSelect();
            SaveLoadService.Instance.PlayerData.WeaponSelectedIndex = _index;
            SaveLoadService.Instance.Save();
            swords[SaveLoadService.Instance.PlayerData.WeaponSelectedIndex].ChoseWeapon();


            TextSelected();
        }
        else
        {
            if(MoneyStorage.Instance.IsValidTransaction(-_cost))
            {
                MoneyStorage.Instance.AddMoney(-_cost);

                SaveLoadService.Instance.PlayerData.BoughtWeapons.Add(_index);
                SaveLoadService.Instance.Save();
                swords[_index].Unlock();
                ButtonConfirm();
                _isBought = true;
            }
        }
    }

    private void ButtonConfirm()
    {
        textSelected.gameObject.SetActive(false);

        button.SetActive(true);
        textButton.text = "Применить";


    }

    private void ButtonBuy()
    {
        textSelected.gameObject.SetActive(false);

        button.SetActive(true);
        textButton.text = "Купить";
    }

    private void TextSelected()
    {
        button.SetActive(false);

        textSelected.gameObject.SetActive(true);
    }

}
