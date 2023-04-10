using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCell : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTxt = null;
    [SerializeField] private TMP_Text isReadyTxt = null;

    private string playerName = null;
    private bool isReady = false;
    private bool isLeft = false;

    public void SetInfo(string playerName, bool isReady, bool isLeft)
    {
        this.playerName = playerName;
        this.isReady = isReady;
        this.isLeft = isLeft;

        playerNameTxt.text = this.playerName;
        isReadyTxt.text = this.isReady ? "Ready" : "";
    }
}
