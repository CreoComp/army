using TMPro;
using UnityEngine;

public class UpgradeHealthButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private TextMeshProUGUI textCharacteristics;

    private void Start()
    {
        UpdateText(SaveLoadService.Instance.PlayerData.HealthLevel);
    }

    public void UpgradeHealth()
    {
        int cost = SaveLoadService.Instance.PlayerData.HealthLevel * 50;

        if (MoneyStorage.Instance.IsValidTransaction(-cost))
        {
            MoneyStorage.Instance.AddMoney(-cost);
            SaveLoadService.Instance.PlayerData.HealthLevel++;
            SaveLoadService.Instance.Save();
            UpdateText(SaveLoadService.Instance.PlayerData.HealthLevel);

        }

    }

    private void UpdateText(int LevelValue)
    {
        textCost.text = LevelValue * 50 + "";
        textCharacteristics.text = LevelValue * 5 + " => " + (LevelValue + 1) * 5;
    }
}
