using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RoomListPanel : MonoBehaviour, Ipanel
{

    [SerializeField] private LobbyManager lobbyManager = null;

    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private RoomCell roomCellPrefab = null;

    [SerializeField] private Transform contentTrans = null;

    private List<RoomCell> roomCells = new List<RoomCell>();

    public void DisplayPanel(bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }

    public void UpdateRoomList(List<SessionInfo> sessionList)
    {
        foreach (Transform child in contentTrans)
        {
            Destroy(child.gameObject);
        }

        roomCells.Clear();

        foreach (var session in sessionList)
        {
            var cell = Instantiate(roomCellPrefab, contentTrans);

            cell.SetInfo(lobbyManager, session.Name, session.PlayerCount);
        }
    }

    public void OnCreateRoomBtnClick()
    {
        lobbyManager.SetPairState(PairState.CreatingRoom);
    }

    // create 5 default room
    private void Start()
    {
        roomCells.Add(new RoomCell("room1", 0));
        roomCells.Add(new RoomCell("room2", 0));
        roomCells.Add(new RoomCell("room3", 0));
        roomCells.Add(new RoomCell("room4", 0));
        roomCells.Add(new RoomCell("room5", 0));
        
        foreach (var roomcell in roomCells)
        {
            var cell = Instantiate(roomCellPrefab, contentTrans);
            cell.SetInfo(lobbyManager, roomcell.roomName, roomcell.playerCount);
        }
    }
    public int ExistedSession(string name, List<SessionInfo> sessionList)
    {
        
        foreach(var session in sessionList)
        {
            if(session.Name == name)
                return session.PlayerCount;
        }
        return 0;
    }

    public void UpdateAvailableRoomList(List<SessionInfo> sessionList)
    {
        foreach (Transform child in contentTrans)
        {
            Destroy(child.gameObject);
        }

        foreach (var roomcell in roomCells)
        {
            var cell = Instantiate(roomCellPrefab, contentTrans);
            cell.SetInfo(lobbyManager, roomcell.roomName, ExistedSession(roomcell.roomName, sessionList));
        }
    }

    public void ExitLobby()
    {
        SceneManager.LoadSceneAsync("TestShopScene");
        RunnerManager.Instance.LeaveSession();
    }
}
