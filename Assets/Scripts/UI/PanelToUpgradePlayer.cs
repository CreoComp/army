using System.Collections;
using TMPro;
using UnityEngine;

public class PanelToUpgradePlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textProgress;
    [SerializeField] private TextMeshProUGUI textName;

    private int level;
    private float progress;

    private void Start()
    {
        level = SaveLoadService.Instance.PlayerData.NowLevel;
        int progressCount = level / 10;

        progress = (level - 1 / 10f) * 100f - 100 * progressCount;

        if (level <= 10)
        {
            textName.text = "Автоматический лазер";
        }
        else if (level <= 20)
        {
            textName.text = "Огненный шар";
        }

        if(level == 10)
        {
            SaveLoadService.Instance.PlayerData.isUseLazer = true;
        }
        else if(level == 20)
        {
            SaveLoadService.Instance.PlayerData.isUseFireBall = true;

        }
       StartCoroutine(AddProgress());
        SaveLoadService.Instance.PlayerData.NowLevel++;
        SaveLoadService.Instance.Save();
    }

    IEnumerator AddProgress()
    {
        for(int i = 1; i <= 10; i ++)
        {
            progress += 1;
            textProgress.text = progress + "%";
            yield return new WaitForSeconds(.2f + (1/i));
        }
    }
}
