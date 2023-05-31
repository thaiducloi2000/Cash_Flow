using Fusion;
using Fusion.Sockets;
using FusionExamples.FusionHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PairState
{
    Lobby,
    CreatingRoom,
    InRoom
}

public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static LobbyManager instance;
    public PairState pairState = PairState.Lobby;

    private RunnerManager gameManager = null;

    [SerializeField] private PlayerNetworkData playerNetworkDataPrefab;

    [SerializeField] private RoomListPanel roomListPanel = null;
    [SerializeField] private CreateRoomPanel createRoomPanel = null;
    [SerializeField] private InRoomPanel inRoomPanel = null;

    public List<SessionInfo> sessions = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        if (sessions == null)
            sessions = new List<SessionInfo>();
    }

    private async void Start()
    {
        SetPairState(PairState.Lobby);

        gameManager = RunnerManager.Instance;

        gameManager.Runner.AddCallbacks(this);

        await JoinLobby(gameManager.Runner);
    }

    public async Task JoinLobby(NetworkRunner runner)
    {
        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);

        if (!result.Ok)
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
    }

    public void SetPairState(PairState newState)
    {
        pairState = newState;

        switch (pairState)
        {
            case PairState.Lobby:
                SetPanel(roomListPanel);
                break;
            case PairState.CreatingRoom:
                SetPanel(createRoomPanel);
                break;
            case PairState.InRoom:
                SetPanel(inRoomPanel);
                break;
        }
    }

    private void SetPanel(Ipanel panel)
    {
        roomListPanel.DisplayPanel(false);
        createRoomPanel.DisplayPanel(false);
        inRoomPanel.DisplayPanel(false);

        panel.DisplayPanel(true);
    }

    public async Task CreateRoom(string roomName, int maxPlayer)
    {
        var result = await gameManager.Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            PlayerCount = maxPlayer,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameManager.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            //ObjectPool = gameManager.gameObject.AddComponent<FusionObjectPoolRoot>()
        });

        if (result.Ok)
            SetPairState(PairState.InRoom);
        else
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
    }

    public async Task JoinRoom(string roomName)
    {
        var result = await gameManager.Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameManager.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            //ObjectPool = gameManager.gameObject.AddComponent<FusionObjectPoolRoot>()
        });

        if (result.Ok)
            SetPairState(PairState.InRoom);
        else
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        runner.Spawn(playerNetworkDataPrefab, Vector3.zero, Quaternion.identity, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (gameManager.PlayerList.TryGetValue(player, out PlayerNetworkData networkPlayerData))
        {
            runner.Despawn(networkPlayerData.Object);

            gameManager.PlayerList.Remove(player);
            gameManager.UpdatePlayerList();
        }
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //roomListPanel.UpdateRoomList(sessionList);
        //roomListPanel.UpdateAvailableRoomList(sessionList);

        
        roomListPanel.UpdateAvailableRoomList(sessionList);
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
       
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }
    private NetworkRunner _runner = null;
    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");

        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        // Step 2.2
        // Create a new Runner.
        if (_runner == null)
        {
            GameObject rm = new GameObject("session");
            _runner = rm.AddComponent<NetworkRunner>();

            _runner.ProvideInput = true;
        }

        //var newRunner = Instantiate(_runner);
        ////_runner.ProvideInput = true;
        //var sceneManager = GetSceneManager(runner);
        // setup the new runner...

        // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
        StartGameResult result = await _runner.StartGame(new StartGameArgs()
        {
            // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
            // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
            SceneManager = gameManager.gameObject.AddComponent<NetworkSceneManagerDefault>(),
            HostMigrationToken = hostMigrationToken,   // contains all necessary info to restart the Runner
            HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
                                                       // other args
        });

        // Check StartGameResult as usual
        if (result.Ok == false)
        {
            Debug.LogWarning(result.ShutdownReason);
        }
        else
        {
            Debug.Log("Done");
        }
    }

    void HostMigrationResume(NetworkRunner runner)
    {

    }

    public void LeaveSession()
    {
        if(_runner != null)
        {
            _runner.Shutdown();
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

   

    
}
