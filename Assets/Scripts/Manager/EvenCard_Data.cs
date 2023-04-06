using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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
                if (card.Image_url != null && card.Image_url != "")
                {
                    //string catchFile = Path.Combine(Application.temporaryCachePath, card.id+".png");
                    //if (File.Exists(catchFile))
                    //{
                    //    byte[] imageData = File.ReadAllBytes(catchFile);
                    //    Texture2D texture = new Texture2D(1024, 756);
                    //    texture.LoadImage(imageData);
                    //    Sprite image_card = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));
                    //    switch (card.Event_type_id)
                    //    {
                    //        case 1:
                    //            LoadBigDeal(card, image_card);
                    //            break;
                    //        case 2:
                    //            LoadSmallDeal(card, image_card);
                    //            break;
                    //        case 3:
                    //            LoadDoodad(card, image_card);
                    //            break;
                    //        case 4:
                    //            LoadMarket(card, image_card);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    //else
                    //{
                    //try
                    //{
                    //    StartCoroutine(helper.DownloadImage(card.Image_url.ToString(), (Sprite) =>
                    //    {
                                Sprite sprite = Sprite.Create(new Texture2D(512, 382), new Rect(0, 0, 512, 382), new Vector2(0f, 0f));
                                switch (card.Event_type_id)
                                {
                                    case 1:
                                        LoadBigDeal(card, sprite);
                                        break;
                                    case 2:
                                        LoadSmallDeal(card, sprite);
                                        break;
                                    case 3:
                                        LoadDoodad(card, sprite);
                                        break;
                                    case 4:
                                        LoadMarket(card, sprite);
                                        break;
                                    default:
                                        break;
                                }
                            //}
                    //    ));
                    //}
                    //catch (Exception e)
                    //{
                    //    Debug.LogError(e.Message);
                    //}
                    //}
                }
            }
        }
        ));

    }

    private void LoadDoodad(Event_card_Entity card,Sprite image)
    {
        Doodads.Add(new Doodad(image, card.Event_name, card.Account_name,card.Event_description, card.Cost,card.Action));
    }


    private void LoadMarket(Event_card_Entity card, Sprite image)
    {
        Markets.Add(new Market(image, card.Event_name, card.Account_name,card.Event_description, card.Cost,card.Action));
    }

    private void LoadBigDeal(Event_card_Entity card, Sprite image)
    {
        Big_Deal_List.Add(new Big_Deal(image, card.Event_name,card.Account_name,card.Event_description, card.Cost, card.Down_pay, card.Trading_range,card.Dept, card.Cash_flow,card.Action));
    }

    private void LoadSmallDeal(Event_card_Entity card, Sprite image)
    {   
        Small_Deal_List.Add(new Small_Deal(image, card.Event_name, card.Account_name,card.Event_description, card.Cost, card.Dept, card.Cash_flow, card.Down_pay,card.Trading_range,card.Action));
    }
}
