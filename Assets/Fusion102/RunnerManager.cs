using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour
{
    public static RunnerManager Instance { get; private set; }

    [SerializeField] private NetworkRunner runner = null;

    public NetworkRunner Runner
    {
        get
        {
            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();

                runner.ProvideInput = true;
            }

            return runner;
        }
    }

    public string PlayerName = null;

    public Dictionary<PlayerRef, PlayerNetworkData> PlayerList = new Dictionary<PlayerRef, PlayerNetworkData>();

    public event Action OnPlayerListUpdated = null;

    private void Awake()
    {
        Runner.ProvideInput = true;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private bool CheckAllPlayerIsReady()
    {
        if (!Runner.IsServer) return false;

        foreach (var playerData in PlayerList.Values)
        {
            if (!playerData.IsReady) return false;
        }

        foreach (var playerData in PlayerList.Values)
        {
            playerData.IsReady = false;
        }
        Debug.Log("All player is ready");
        return true;
    }

    public void UpdatePlayerList()
    {
        OnPlayerListUpdated?.Invoke();
        
        if (CheckAllPlayerIsReady())
        {
            Debug.Log("All player is ready yet");
            Runner.SetActiveScene("GamePlay_Scene");
        }

    }

    public void SetPlayerNetworkData()
    {
        if (PlayerList.TryGetValue(runner.LocalPlayer, out PlayerNetworkData playerNetworkData))
        {
            playerNetworkData.SetPlayerName_RPC(PlayerName);
        }
    }
}
