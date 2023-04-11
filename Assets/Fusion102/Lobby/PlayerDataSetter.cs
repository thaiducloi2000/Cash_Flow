using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataSetter : NetworkBehaviour
{
    private RunnerManager gameManager = null;
    public TMP_InputField playername;
    public TMP_Text playerName;
    public User_Data user;

    private void Start()
    {
        gameManager = RunnerManager.Instance;
        gameManager.PlayerName = user.data.user.NickName;
        gameManager.SetPlayerNetworkData();
        playerName.text = gameManager.PlayerName.ToString();
    }

    public void OnPlayerNameInputFieldChange(string value)
    {
        gameManager.PlayerName = value;

        gameManager.SetPlayerNetworkData();
    }
}
