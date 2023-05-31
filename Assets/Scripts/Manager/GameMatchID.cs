using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMatchID : NetworkBehaviour
{
    public static GameMatchID Instance;

    public string matchID;

    public bool endGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;

        endGame = false;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MatchID(string id, RpcInfo info = default)
    {
        this.matchID = id;
        Debug.Log("MatchID : " + matchID);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndGame(bool endGame, RpcInfo info = default)
    {
        this.endGame = endGame;
        Debug.Log("EndGame : " + this.endGame);
    }
}
