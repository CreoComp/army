using System;

public class MoneyStorage 
{
    private static MoneyStorage instance;
    public static MoneyStorage Instance
    {
        get
        {
            if(instance == null)
                instance = new MoneyStorage();
            return instance;
        }
    }

    public Action<int> ChangedMoneyAmount;

    public void AddMoney(int amount)
    {
        if(IsValidTransaction(amount))
        {
            SaveLoadService.Instance.PlayerData.Money += amount;
            SaveLoadService.Instance.Save();
            ChangedMoneyAmount?.Invoke(SaveLoadService.Instance.PlayerData.Money);
        }
    }

    public int GetMoney() => SaveLoadService.Instance.PlayerData.Money;

    public bool IsValidTransaction(int amount) =>
        SaveLoadService.Instance.PlayerData.Money + amount >= 0;
}
