using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InRoomPanel : MonoBehaviour, Ipanel
{
    private RunnerManager gameManager = null;

    [SerializeField] private LobbyManager lobbyManager = null;

    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private TMP_Text roomNameTxt = null;

    [SerializeField] private PlayerCell playerCellPrefab = null;
    [SerializeField] private Transform contentTrans = null;

    private List<PlayerCell> playerCells = new List<PlayerCell>();

    private NetworkRunner runner;

    private void Start()
    {
        gameManager = RunnerManager.Instance;

        gameManager.OnPlayerListUpdated += UpdatePlayerList;

        runner = gameManager.Runner;
        
    }

    private void OnDestroy()
    {
        gameManager.OnPlayerListUpdated -= UpdatePlayerList;
    }

    public void UpdatePlayerList()
    {
        foreach (var cell in playerCells)
        {
            Destroy(cell.gameObject);
        }

        playerCells.Clear();

        foreach (var player in gameManager.PlayerList)
        {
            var cell = Instantiate(playerCellPrefab, contentTrans);

            var playerData = player.Value;

            cell.SetInfo(playerData.PlayerName, playerData.IsReady, playerData.IsLeftRoom);

            playerCells.Add(cell);

            if (playerData.IsLeftRoom)
            {
                playerCells.Remove(cell);
                Destroy(cell.gameObject);
            }

        }
    }

    public void DisplayPanel(bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;

        if(runner != null)
        {
            roomNameTxt.text = runner.SessionInfo.Name;
        }
       
    }

    public void OnReadyBtnClicked()
    {
        var runner = gameManager.Runner;

        if (gameManager.PlayerList.TryGetValue(runner.LocalPlayer, out PlayerNetworkData playerNetworkData))
        {
            playerNetworkData.SetReady_RPC(true);
        }
    }

    public void OnLeaveRoomClicked()
    {
        var runner = gameManager.Runner;

        //if (runner.IsServer)
        //{
        //    runner.Shutdown();
        //}

        if (gameManager.PlayerList.TryGetValue(runner.LocalPlayer, out PlayerNetworkData playerNetworkData))
        {
            playerNetworkData.LeftRoom_RPC(true);
        }

        gameManager.OnPlayerListUpdated -= UpdatePlayerList;

        lobbyManager.SetPairState(PairState.Lobby);
    }
}
