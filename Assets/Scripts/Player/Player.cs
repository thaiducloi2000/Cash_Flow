using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

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
    public Job job;
    public Turn myTurn;
    public List<Dream> dreams;

    [Networked (OnChanged = nameof(OnStepChanged))]
    public int step { get; set; }


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Instance = this;
            LoadAllJob();
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
        offset = new Vector3(GameBoard.Instance.size * 0f, GameBoard.Instance.size * 3f, -10f);
        //offset = new Vector3(0, 7, -9f);
        Camera.main.transform.position = offset;
        //Camera.main.transform.LookAt(root);

        //LoadAllJob();
    }


    void Update()
    {
        Camera.main.transform.LookAt(root);
        MoveCameraOnCirle();
    }

    public void OnRollDice()
    {
        if (Object.HasInputAuthority)
        {
            RPC_Random_Dice();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Random_Dice(RpcInfo info = default)
    {
        step = Random.Range(1, 7);
    }

    public void SelectJoB()
    {
        UI_Manager.instance.PopupJob_UI(EvenCard_Data.instance.Job_List[Random.Range(0, EvenCard_Data.instance.Job_List.Count - 1)]);
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
        this.financial_rp_fat_race = new Financial(0, this.id, this.job.id, this.financial_rp.GetPassiveIncome() * 100f, 0, list);

        UI_Manager.instance.Financial_Panel.GetComponent<Financial_Panel_Manager>().Financial(this.financial_rp_fat_race);
        if (Object.HasInputAuthority)
            StartCoroutine(this.gameObject.GetComponent<Step>().MoveFatRace(this));
    }

    private void LoadAllJob()
    {
        EvenCard_Data.instance.Job_List = new List<Job>();
        StartCoroutine(EvenCard_Data.instance.helper.Get("jobcards/all", (request, process) =>
        {
            List<Job> list = EvenCard_Data.instance.helper.ParseToList<Job>(request);
            foreach (Job job in list)
            {
                EvenCard_Data.instance.Job_List.Add(job);
            }
            if (request.downloadProgress == 1)
            {
                SelectJoB();
            }
        }
        ));
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

        GameManager.Instance.nextTurn();
    }
}
