using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCell : MonoBehaviour
{
    public string roomName = null;
    public int playerCount = 0;


    private LobbyManager lobbyManager = null;

    [SerializeField] private TMP_Text roomNameTxt = null;
    [SerializeField] private TMP_Text playerCountTxt = null;
    [SerializeField] private Button joinBtn = null;



    //set 5 default room
    public RoomCell(string roomName, int playerCount)
    {
        this.roomName = roomName;
        this.playerCount = playerCount;
    }

    private void Update()
    {
        
        Debug.Log("Room name: " + roomNameTxt.text.ToString());
    }

    public void SetInfo(LobbyManager lobbyManager, string roomName, int playerCount)
    {
        this.lobbyManager = lobbyManager;
        roomNameTxt.text = roomName;
        playerCountTxt.text = $"{playerCount}/4";
        this.roomName = roomName;

        bool isJoinButtonActive = true;

        if (playerCount >= 4)
        {
            isJoinButtonActive = false;
        }

        joinBtn.gameObject.SetActive(isJoinButtonActive);
    }

    public async void OnJoinBtnClicked()
    {
        await lobbyManager.JoinRoom(roomName);
    }
}
