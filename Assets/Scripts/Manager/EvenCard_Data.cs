using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Event_card_Entity;

public class EvenCard_Data : MonoBehaviour
{
    public static EvenCard_Data instance;
    [SerializeField] public List<Job> Job_List;
    [SerializeField] public List<Small_Deal> Small_Deal_List;
    [SerializeField] public List<Big_Deal> Big_Deal_List;
    [SerializeField] public List<Doodad> Doodads;
    [SerializeField] public List<Market> Markets;
    public Server_Connection_Helper helper;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
        if (helper == null)
        {
            helper = GetComponent<Server_Connection_Helper>();
        }
        LoadAllDeal();
        //LoadAllJob();
    }

    private void Update()
    {
        
    }

    private void LoadAllDeal()
    {
        // load all Deal from db
        Small_Deal_List = new List<Small_Deal>();
        Big_Deal_List = new List<Big_Deal>();
        Doodads = new List<Doodad>();
        Markets = new List<Market>();
        StartCoroutine(helper.Get("eventcards/all", (request,process) =>
        {
            List<Event_card_Entity> event_card = helper.ParseToList<Event_card_Entity>(request);
            //Debug.Log(string.Format("Downloaded Event Card Process {0:P1}", process * 100f + "%"));
            foreach (Event_card_Entity card in event_card)
            {
                switch (card.Event_type_id)
                {
                    case 1:
                        LoadBigDeal(card);
                        break;
                    case 2:
                        LoadSmallDeal(card);
                        break;
                    case 3:
                        LoadDoodad(card);
                        break;
                    case 4:
                        LoadMarket(card);
                        break;
                    default:
                        break;
                }
            }
        }
        ));

    }

    private void LoadDoodad(Event_card_Entity card)
    {
        Doodads.Add(new Doodad(card.Event_name, card.Account_Name,card.Event_description, card.Cost,card.Action));
    }


    private void LoadMarket(Event_card_Entity card)
    {
        Markets.Add(new Market(card.Event_name, card.Account_Name,card.Event_description, card.Cost,card.Action));
    }

    private void LoadBigDeal(Event_card_Entity card)
    {
        Big_Deal_List.Add(new Big_Deal(card.Event_name,card.Account_Name,card.Event_description, card.Cost, card.Down_pay, card.Trading_range,card.Dept, card.Cash_flow,card.Action));
    }

    private void LoadSmallDeal(Event_card_Entity card)
    {   
        Small_Deal_List.Add(new Small_Deal(card.Event_name, card.Account_Name,card.Event_description, card.Cost, card.Dept, card.Cash_flow, card.Down_pay,card.Trading_range,card.Action));
    }
}
