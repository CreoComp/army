using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI _textMoney;

    private void Start()
    {
        _textMoney = GetComponent<TextMeshProUGUI>();
        ChangeText(MoneyStorage.Instance.GetMoney());
    }

    private void OnEnable()
    {
        MoneyStorage.Instance.ChangedMoneyAmount += ChangeText;
    }

    private void OnDisable()
    {
        MoneyStorage.Instance.ChangedMoneyAmount -= ChangeText;
    }

    private void ChangeText(int money)
    {
        _textMoney.text = money + "";
    }
}
