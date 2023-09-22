using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuyWeaponSkin : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private int _cost;
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private GameObject lockPanel;

    [SerializeField] private Color _colorSelected;
    [SerializeField] private Color _colorChose;

    private Outline outline;

    private Inventory inventory;
    public void Construct()
    {
        inventory = transform.parent.GetComponent<Inventory>();
        textCost.text = _cost + "";
        outline = GetComponent<Outline>();
        UnSelect();
    }

    public void Click()
    {
        inventory.SelectWeapon(index, _cost);
    }

    public void Unlock()
    {
        lockPanel.SetActive(false);
    }

    public void ChoseWeapon()
    {
        outline.enabled = true;

        outline.effectColor = _colorChose;
    }

    public void Select()
    {
        outline.enabled = true;

        outline.effectColor = _colorSelected;
    }
    public void UnSelect()
    {
        outline.enabled = false;
    }
}