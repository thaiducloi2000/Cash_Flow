using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn { A,B,C,D}

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;

    public GameObject PlayerPrefabs;

    public List<Player> playerList;
    public bool isPlayerMoving;
    public bool EndGame;
    // To Defind Player Turn;
    public Turn isTurn;


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
            Instantiate(PlayerPrefabs);
        //}
        EndGame = false;
    }

    private void Start()
    {
        this.isTurn = Turn.A;
        playerList = new List<Player>();
        FindAllPlayerInScene();
        MoveToStartPoint();
    }


    public void FindAllPlayerInScene()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
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
        }
    }

    public void MoveToStartPoint()
    {
        foreach(Player player in playerList)
        {
            if(player.gameObject.GetComponent<Player>() != null)
            {
                player.Avatar.transform.localScale *= GameBoard.Instance.size;
                Vector3 pos = new Vector3(GameBoard.Instance.Tiles_Rat_Race[0].transform.position.x,0.25f, GameBoard.Instance.Tiles_Rat_Race[0].transform.position.z);
                player.Avatar.transform.position = pos;
            }
        }
    }

    public void nextTurn()
    {
        // switch turn to next player
        switch (isTurn)
        {
            case Turn.A:
                this.isTurn = Turn.B;
                break;
            case Turn.B:
                this.isTurn = Turn.C;
                break;
            case Turn.C:
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
