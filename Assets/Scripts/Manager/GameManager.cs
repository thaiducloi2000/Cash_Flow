using Fusion;
using Fusion.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum Turn { A,B,C,D,None}

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

    // To Defind Player Turn;

    public Turn isTurn;

    public string matchID;

    [Networked]
    public int totalPlayer { get; set; }

    public GameMatchID gameMatchID;

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
    }

    private void Start()
    {
        //StartGame(GameMode.AutoHostOrClient);
        gameBoard = GameBoard.Instance;
        gameManager = RunnerManager.Instance;

        _runner = gameManager.Runner;

        _runner.AddCallbacks(this);

        SpawnAllPlayers();

        if (_runner.GameMode == GameMode.Host)
            CreateMatch();

        this.isTurn = Turn.A;
        playerList = new List<Player>();
        FindAllPlayerInScene();
        //MoveToStartPoint();
        
    }

    public void CreateMatch()
    {
        Server_Connection_Helper helper = this.gameObject.GetComponent<Server_Connection_Helper>();
        WWWForm form = new WWWForm();
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("MaxNumberPlayer", totalPlayer);
        data.Add("WinnerId", null); 
        data.Add("LastHostId", null); ;
        data.Add("TotalRound", "1");
        data.Add("gameModId", "1");
        string bodydata = JsonConvert.SerializeObject(data);
        StartCoroutine(helper.Post("gamematches", form, bodydata, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    this.matchID = request.downloadHandler.text;        
                    gameMatchID.RPC_MatchID(this.matchID);
                    break;
                default:
                    break;
            }
        }));
    }

    public void SaveFinancial(int childAmount,int totalStep,float totalMoney,bool isWin,int Score,float IncomePerMonth, float ExpensePerMonth,int coint,int point,int userID)
    {
        this.matchID = gameMatchID.matchID.ToString();
        Server_Connection_Helper helper = this.gameObject.GetComponent<Server_Connection_Helper>();
        WWWForm form = new WWWForm();
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("ChildrenAmount", childAmount);
        data.Add("TotalStep", totalStep);
        data.Add("TotalMoney", totalMoney); ;
        data.Add("IsWin", isWin);
        data.Add("Score", Score);
        data.Add("IncomePerMonth", IncomePerMonth);
        data.Add("ExpensePerMonth", ExpensePerMonth);
        data.Add("MatchId", this.matchID);
        string bodydata_1 = JsonConvert.SerializeObject(data);
        StartCoroutine(helper.Post("gamereports", form, bodydata_1, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Save Sucessfull : " + request.downloadHandler.text);
                    break;
                default:
                    break;
            }
        }));
        Dictionary<string, int> parameter = new Dictionary<string, int>();

        Dictionary<string, object> data_1 = new Dictionary<string, object>();
        parameter.Add("userId",userID);
        data_1.Add("Coin",coint);
        data_1.Add("Point",point);
        string bodydata_2 = JsonConvert.SerializeObject(data_1);
        StartCoroutine(helper.Put_Parameter("users/coin-point", parameter, bodydata_2, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Save Sucessfull : " + request.downloadHandler.text);
                    Player.Instance.user_data.data.user.Coin = coint;
                    Player.Instance.user_data.data.user.Point = point;
                    break;
                default:
                    break;
            }
        }));

    }

    // Call When have A winner
    public void UpdateMatchWiner(int userID,int totalStep)
    {
        this.matchID = gameMatchID.matchID.ToString();
        Server_Connection_Helper helper = this.gameObject.GetComponent<Server_Connection_Helper>();
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("MaxNumberPlayer", totalPlayer);
        data.Add("WinnerId", userID);
        data.Add("LastHostId", userID);
        data.Add("TotalRound", totalStep);
        data.Add("gameModId", "1");
        string bodydata = JsonConvert.SerializeObject(data);
        StartCoroutine(helper.Put_Parameter_Single("gamematches",this.matchID, bodydata, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Winner ID : " + userID);
                    break;
                default:
                    break;
            }
        }));
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
                    player.GetComponent<Player>().infoNumber = 0;
                    break;
                case 1:
                    player.GetComponent<Player>().myTurn = Turn.B;
                    player.GetComponent<Player>().infoNumber = 1;
                    break;
                case 2:
                    player.GetComponent<Player>().myTurn = Turn.C;
                    player.GetComponent<Player>().infoNumber = 2;
                    break;
                case 3:
                    player.GetComponent<Player>().myTurn = Turn.D;
                    player.GetComponent<Player>().infoNumber = 3;
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

 
    public void RPC_nextTurn(Turn playerTurn)
    {
        // switch turn to next player
        switch (playerTurn)
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

    public void CheckPlayerWinner()
    {
        //Player.Instance.GetComponent<Player>().ShowResult();
        FinishPanel.instance.RPC_Lose();
        FinishPanel.instance.Win();
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
