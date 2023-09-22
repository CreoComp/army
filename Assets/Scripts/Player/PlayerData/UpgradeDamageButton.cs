using TMPro;
using UnityEngine;

public class UpgradeDamageButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private TextMeshProUGUI textCharacteristics;

    private void Start()
    {
        UpdateText(SaveLoadService.Instance.PlayerData.DamageLevel);
    }
    public void UpgradeDamage()
    {
        int cost = SaveLoadService.Instance.PlayerData.DamageLevel * 50;

        if (MoneyStorage.Instance.IsValidTransaction(-cost))
        {
            MoneyStorage.Instance.AddMoney(-cost);
            SaveLoadService.Instance.PlayerData.DamageLevel++;
            SaveLoadService.Instance.Save();
            UpdateText(SaveLoadService.Instance.PlayerData.DamageLevel);

        }
    }

    private void UpdateText(int LevelValue)
    {
        textCost.text = LevelValue * 50 + "";
        textCharacteristics.text = LevelValue * 2 + " => " + (LevelValue + 1) * 2;
    }
}
