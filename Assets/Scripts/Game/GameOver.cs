using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject GameDefeatPanel;
    public GameObject GameWinPanel;

    public GameObject panlUpgrade;

    public static GameOver instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance == this)
            Destroy(gameObject);
    }

    public void Defeat()
    {
        GameDefeatPanel.SetActive(true);
    }

    public void Win()
    {
        StartCoroutine(WinTimer());
    }

    IEnumerator WinTimer()
    {
        if (SaveLoadService.Instance.PlayerData.NowLevel <= 20)
        {
            panlUpgrade.SetActive(true);
            yield return new WaitForSeconds(5);
            panlUpgrade.SetActive(false);
            GameWinPanel.SetActive(true);

        }
        else
        {
            GameWinPanel.SetActive(true);
        }
    }
}
