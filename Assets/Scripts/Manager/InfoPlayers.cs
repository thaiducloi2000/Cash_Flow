using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayers : NetworkBehaviour
{
    public static InfoPlayers Instance { get; private set; }

    public GameObject[] infoPlayers;
    public GameManager gameManager;
    public InfoPlayer infoPlayer1;
    public InfoPlayer infoPlayer2;
    public InfoPlayer infoPlayer3;
    public InfoPlayer infoPlayer4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for(int i = 0; i < gameManager.totalPlayer; i++)
        {
            infoPlayers[i].gameObject.SetActive(true);
        }
    }

    public void SetInfoPlayers(int infoNumber, string name, string job, float money, int baby)
    {
        switch (infoNumber)
        {
            case 0:
                infoPlayers[0].GetComponent<InfoPlayer>().RPC_SetInformation(name, job, money, baby);
                break;
            case 1:
                infoPlayers[1].GetComponent<InfoPlayer>().RPC_SetInformation(name, job, money, baby);
                break;
            case 2:
                infoPlayers[2].GetComponent<InfoPlayer>().RPC_SetInformation(name, job, money, baby);
                break;
            case 3:
                infoPlayers[3].GetComponent<InfoPlayer>().RPC_SetInformation(name, job, money, baby);
                break;
        }
    }
}
