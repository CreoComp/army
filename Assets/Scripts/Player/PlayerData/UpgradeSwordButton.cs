using TMPro;
using UnityEngine;

public class UpgradeSwordButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private TextMeshProUGUI textCharacteristics;

    private void Start()
    {
        UpdateText(SaveLoadService.Instance.PlayerData.SwordLevel);
    }
    public void UpgradeSword()
    {
        int cost = SaveLoadService.Instance.PlayerData.SwordLevel * 200;

        if (MoneyStorage.Instance.IsValidTransaction(-cost))
        {
            MoneyStorage.Instance.AddMoney(-cost);
            SaveLoadService.Instance.PlayerData.SwordLevel++;
            SaveLoadService.Instance.Save();
            UpdateText(SaveLoadService.Instance.PlayerData.SwordLevel);
        }

    }

    private void UpdateText(int LevelValue)
    {
        textCost.text = LevelValue * 200 + "";
        textCharacteristics.text = LevelValue  + " => " + (LevelValue + 1);
    }
}
