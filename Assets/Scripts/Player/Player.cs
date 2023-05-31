using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player Instance { get; set; }
    public string id = "7f1515ce-a0bd-4b95-adf7-98d1243e6117CU";
    public Transform root;
    public Vector3 offset;
    // Add Avatar
    public bool isInFatRace = false;
    public float Children_cost = 0;
    //public float cash;
    public Financial financial_rp;
    public Financial financial_rp_fat_race;
    //public Job job;
    [Networked]
    public Turn myTurn { get; set;}
    public List<Dream> dreams;

    [Networked]
    public int step { get; set; }
    private Dice dice;
    [Networked(OnChanged = nameof(OnStepChanged))]
    public int totalStep { get; set; }
    [Networked]
    public int infoNumber { get; set; }
    
    
    [Networked]
    public NetworkBool result { get; set; }

    public User_Data user_data;

    [Networked]
    public bool lostTurn { get; set; }
    public int noOfTurn;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Instance = this;
            user_data = Resources.Load<User_Data>("Items/User_Data");
            SelectJoB();
            //endGame = false;
            //RPC_LostTurn(false);
            Debug.Log("Spawn local player");
        }
        else
        {
            Debug.Log("Spawn remote player");
        }

    }

    private void Start()
    {
        //setting camera 

        root = GameBoard.Instance.Root.transform;
        offset = new Vector3(GameBoard.Instance.size * 0f, GameBoard.Instance.size * 5f, -9f);
        //offset = new Vector3(0, 7, -9f);
        Camera.main.transform.position = offset;
        Camera.main.transform.LookAt(root);
        dice = Dice.Instance;
        //if(Object.HasInputAuthority)
        //    RPC_HideDice(false);

    }


    void Update()
    {
        //Camera.main.transform.LookAt(root);
        MoveCameraOnCirle();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            //if (GameManager.Instance.isTurn != myTurn)
            //    UI_Manager.instance.Roll_Button.SetActive(false);
            if (GameManager.Instance.isTurn == myTurn)
                UI_Manager.instance.Roll_Button.SetActive(true);
            
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_LostTurn(bool lostTurn, RpcInfo info = default)
    {
        this.lostTurn = lostTurn;
    }

    public void OnRollDice()
    {
        if (Object.HasInputAuthority)
        {
            RPC_HideDice(true);
            RPC_Random_Dice();
        }
        GameManager.Instance.isTurn = Turn.None;
        UI_Manager.instance.Roll_Button.SetActive(false);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Random_Dice(RpcInfo info = default)
    {
        //step = Random.Range(1, 7);
        StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        //int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);
            //RPC_RandomNumber(randomDiceSide);

            // Set sprite to upper face of dice from array according to random value
            // dice.rend.sprite = dice.diceSides[randomDiceSide];
            RPC_DisplayDice(randomDiceSide);

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        step = randomDiceSide + 1;
        totalStep += step;
        // Show final dice value in Console
        Debug.Log(step);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_DisplayDice(int randomDiceSide, RpcInfo info = default)
    {
        dice.rend.sprite = dice.diceSides[randomDiceSide];
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_HideDice(NetworkBool isHide, RpcInfo info = default)
    {
        dice.rend.gameObject.SetActive(isHide);
    }

    public void SelectJoB()
    {
        UI_Manager.instance.PopupJob_UI(user_data.LastJobSelected);
    }

    public void MoveCameraOnCirle()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Camera.main.transform.RotateAround(root.transform.position, new Vector3(0f, touch.deltaPosition.x, 0f), 100f * Time.deltaTime);
            }
        }
    }

    public void MoveToFatRace()
    {
        this.isInFatRace = true;
        this.gameObject.GetComponent<Step>().currentPos = 0;
        Game_accounts account = new Game_accounts();
        account.Game_account_value = financial_rp.GetPassiveIncome() * 100f;
        account.Game_account_type = AccountType.Income;
        account.Game_account_name = "Salary";
        Game_accounts account_2 = new Game_accounts();
        account_2.Game_account_value = financial_rp.GetPassiveIncome() * 50f;
        account_2.Game_account_type = AccountType.Income;
        account_2.Game_account_name = "Cash";
        List<Game_accounts> list = new List<Game_accounts>();
        list.Add(account);
        list.Add(account_2);
        this.financial_rp_fat_race = new Financial(0, this.id, this.user_data.LastJobSelected.id, this.financial_rp.GetPassiveIncome() * 100f, 0, list);

        UI_Manager.instance.Financial_Panel.GetComponent<Financial_Panel_Manager>().Financial(this.financial_rp_fat_race);
        if (Object.HasInputAuthority)
            StartCoroutine(this.gameObject.GetComponent<Step>().MoveFatRace(this));
    }

    static void OnStepChanged(Changed<Player> changed)
    {
        changed.Behaviour.OnStepChanged();
    }
    

    private void OnStepChanged()
    {
        if (isInFatRace)
        {
            if(Object.HasInputAuthority)
                StartCoroutine(GetComponent<Step>().Move(this, step, BoardType.FatRace));
        }
        else
        {
            if (Object.HasInputAuthority)
                StartCoroutine(GetComponent<Step>().Move(this, step, BoardType.RatRace));
        }
        //if (Object.HasInputAuthority)
        StartCoroutine("WaitForNextTurn");
        
    }

    public void Save()
    { 
        if (result)
        {
            Debug.Log("Saving ");
            if (Object.HasInputAuthority)
            {
                GameManager.Instance.SaveFinancial(this.financial_rp.children_amount, this.totalStep, this.financial_rp.GetCash(),
                true, 200, this.financial_rp.income_per_month, this.financial_rp.expense_per_month, this.user_data.data.user.Coin + 50,
                this.user_data.data.user.Point + 200, this.user_data.data.user.UserId);
                GameManager.Instance.UpdateMatchWiner(this.user_data.data.user.UserId, this.totalStep);
            }
                
            Debug.Log("Saved ");
        }
        else
        {
            Debug.Log("Saving ");
            if (Object.HasInputAuthority)
            {
                GameManager.Instance.SaveFinancial(this.financial_rp.children_amount, this.totalStep, this.financial_rp.GetCash(),
                false, 50, this.financial_rp.income_per_month, this.financial_rp.expense_per_month, this.user_data.data.user.Coin + 10,
                this.user_data.data.user.Point + 40, this.user_data.data.user.UserId);
            }
            Debug.Log("Saved ");
        }
    }

    public void ShowResult()
    {
        FinishPanel.instance.RPC_Lose();
    }

    private IEnumerator WaitForNextTurn()
    {
        float time = (float) step/2 + 1;
        yield return new WaitForSeconds(time);
        GameManager.Instance.RPC_nextTurn(CheckLostTurn());
    }

    public void EndGame()
    {
        if (Object.HasInputAuthority)
            RPC_Result(true);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_Result(bool result, RpcInfo info = default)
    {
        this.result = result;
    }

    private Turn CheckLostTurn()
    {
        Turn turn = myTurn;
        GameObject[] gameObjectsToTransfer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in gameObjectsToTransfer)
        {
            if (player.GetComponent<Player>().lostTurn)
            {
                turn = player.GetComponent<Player>().myTurn;
                if (Object.HasInputAuthority)
                    RPC_LostTurn(false);
                Debug.Log("TurnLost: " + turn);
                return turn;
            }
            Debug.Log("TurnLost: " + player.GetComponent<Player>().lostTurn);
        }
        return turn;
    }

}
