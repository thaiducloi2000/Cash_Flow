using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn { A,B,C,D}

public class GameManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner = null;

    private RunnerManager gameManager = null;

    [SerializeField] private GameBoard gameBoard = null;

    [SerializeField] private NetworkPrefabRef _playerPrefab; // Character to spawn for a joining player
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    // Start is called before the first frame update
    public static GameManager Instance;

    //public GameObject PlayerPrefabs;

    public List<Player> playerList;
    public bool isPlayerMoving;
    public bool EndGame;
    // To Defind Player Turn;

    public Turn isTurn;

    [Networked]
    public int totalPlayer { get; set; }

    private void Update()
    {
        Debug.Log("End Game : " + EndGame);
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        // change to Instantiate to list when moving to multilplayer
        //for(int i = 0; i < 4; i++)
        //{
            //Instantiate(PlayerPrefabs);
        //}
        EndGame = false;
    }

    private void Start()
    {
        //StartGame(GameMode.AutoHostOrClient);
        gameBoard = GameBoard.Instance;
        gameManager = RunnerManager.Instance;

        _runner = gameManager.Runner;

        _runner.AddCallbacks(this);

        SpawnAllPlayers();

        this.isTurn = Turn.A;
        playerList = new List<Player>();
        FindAllPlayerInScene();
        //MoveToStartPoint();
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void SpawnAllPlayers()
    {
        foreach (var player in gameManager.PlayerList.Keys)
        {
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % _runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            Vector3 spawnPosition = new Vector3(GameBoard.Instance.Tiles_Rat_Race[0].transform.position.x, 0.05f, GameBoard.Instance.Tiles_Rat_Race[0].transform.position.z);
            NetworkObject networkPlayerObject = _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            _runner.SetPlayerObject(player, networkPlayerObject);

            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        Log.Debug("Count: " + gameManager.PlayerList.Count);
        totalPlayer = gameManager.PlayerList.Count;
    }

    public void FindAllPlayerInScene()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        { 
            switch (playerList.Count)
            {
                case 0:
                    player.GetComponent<Player>().myTurn = Turn.A;
                    break;
                case 1:
                    player.GetComponent<Player>().myTurn = Turn.B;
                    break;
                case 2:
                    player.GetComponent<Player>().myTurn = Turn.C;
                    break;
                case 3:
                    player.GetComponent<Player>().myTurn = Turn.D;
                    break;
                default:
                    break;
            }
            playerList.Add(player.gameObject.GetComponent<Player>());
            //Debug.Log("PlayerCount: " + playerList.Count);
        }
    }

    public void MoveToStartPoint()
    {
        foreach(Player player in playerList)
        {
            if(player.gameObject.GetComponent<Player>() != null)
            {
                transform.localScale *= GameBoard.Instance.size;
                Vector3 pos = new Vector3(GameBoard.Instance.Tiles_Rat_Race[0].transform.position.x,0.05f, GameBoard.Instance.Tiles_Rat_Race[0].transform.position.z);
                transform.position = pos;
            }
        }
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_nextTurn()
    {
        // switch turn to next player
        switch (isTurn)
        {
            case Turn.A:
                if (totalPlayer == 1)
                    this.isTurn = Turn.A;
                else if (totalPlayer == 2)
                    this.isTurn = Turn.B;
                break;
            case Turn.B:
                if (totalPlayer == 2)
                    this.isTurn = Turn.A;
                else if (totalPlayer == 3)
                    this.isTurn = Turn.C;
                break;
            case Turn.C:
                if (totalPlayer == 3)
                    this.isTurn = Turn.A;
                else if (totalPlayer == 4)
                    this.isTurn = Turn.D;
                break;
            case Turn.D:
                this.isTurn = Turn.A;
                break;
            default:
                Debug.Log("Invalid Turn");
                break;
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            //Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
            Vector3 spawnPosition = new Vector3(gameBoard.Tiles_Rat_Race[0].transform.position.x, 0.25f, gameBoard.Tiles_Rat_Race[0].transform.position.z);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

}
