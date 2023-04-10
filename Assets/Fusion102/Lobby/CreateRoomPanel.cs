using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateRoomPanel : MonoBehaviour, Ipanel
{
    [SerializeField] private LobbyManager lobbyManager = null;

    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private TMP_InputField roomNameInputField = null;

    public void DisplayPanel(bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }

    public void OnBackBtnClicked()
    {
        lobbyManager.SetPairState(PairState.Lobby);
    }

    public async void OnCreateBtnClicked()
    {
        string roomName = roomNameInputField.text;
        int maxPlayer = 4;
        await lobbyManager.CreateRoom(roomName, maxPlayer);
    }
}
