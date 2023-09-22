using UnityEngine;

public class LoadSaveLoadService : MonoBehaviour
{
    private void Awake()
    {
        SaveLoadService.Instance.Load();
    }
}
