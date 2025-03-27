using UnityEngine;

public class Boot : MonoBehaviour
{
    private const float WaitingTimeSecMax = 2f;

    private void Awake()
    {
        Universe.Create();
        //ConfigLoader configLoader = base.gameObject.AddComponent<ConfigLoader>();
        //configLoader.LoadAll(OnConfigAllUpdated);

        OnCloudLoginFinished();
        MonoSingleton<Cloud>.Instance.StartSyncFlow();
    }

    private void OnConfigAllUpdated(ConfigsData configData)
    {
        App.Instance.Configs.Override(configData);
        MonoSingleton<Cloud>.Instance.CloudSyncFinishedEvent += OnCloudLoginFinished;
        MonoSingleton<Cloud>.Instance.StartSyncFlow();
    }

    private void OnCloudLoginFinished()
    {
        MonoSingleton<Cloud>.Instance.CloudSyncFinishedEvent -= OnCloudLoginFinished;
        LoadGame();
    }

    private void LoadGame()
    {
        App.Instance.LoadMainMenu();
    }
}