using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTurn : NetworkBehaviour
{
    public static CheckTurn Instance;

    public Turn isTurn;
    private GameManager gameManager;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        this.isTurn = Turn.A;
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_nextTurn()
    {
        // switch turn to next player
        switch (isTurn)
        {
            case Turn.A:
                if (gameManager.playerList.Count == 1)
                    this.isTurn = Turn.A;
                else if (gameManager.playerList.Count == 2)
                    this.isTurn = Turn.B;
                break;
            case Turn.B:
                if (gameManager.playerList.Count == 2)
                    this.isTurn = Turn.A;
                else if (gameManager.playerList.Count == 3)
                    this.isTurn = Turn.C;
                break;
            case Turn.C:
                if (gameManager.playerList.Count == 3)
                    this.isTurn = Turn.A;
                else if (gameManager.playerList.Count == 4)
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
}
