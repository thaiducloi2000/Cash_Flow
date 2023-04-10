using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataSetter : MonoBehaviour
{
    private RunnerManager gameManager = null;
    public TMP_InputField playername;

    private void Start()
    {
        gameManager = RunnerManager.Instance;

    }

    public void OnPlayerNameInputFieldChange(string value)
    {
        gameManager.PlayerName = value;

        gameManager.SetPlayerNetworkData();
    }
}
