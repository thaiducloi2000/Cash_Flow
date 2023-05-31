using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Historypanel;
    [SerializeField] private User_Data user;

    [SerializeField] private List<HistoryDTO> histories;
    private Server_Connection_Helper helper;
    [SerializeField] private GameObject Victory_Prefabs;
    [SerializeField] private GameObject Lose_Prefabs;
    [SerializeField] private GameObject content;
    private List<GameObject> prefabs;

    private void Awake()
    {
        this.user = GetComponentInParent<Shop_Data>().user_data;
        prefabs = new List<GameObject>();
        if (helper == null)
        {
            this.gameObject.AddComponent<Server_Connection_Helper>();
            helper = this.gameObject.GetComponent<Server_Connection_Helper>();
            helper.Authorization_Header = "Bearer " + user.data.token;
        }
    }

    private void Start()
    {
        Load_History_Match();
    }


    private void Load_History_Match()
    {
        histories = new List<HistoryDTO>();
        StartCoroutine(helper.Get_Parameter("gamereports/my-report", user.data.user.UserId.ToString(), (request, process) =>
        {
            Debug.Log(request.downloadHandler.text);
            if (histories == null)
            {
                histories = new List<HistoryDTO>();
            }
            histories = helper.ParseToList<HistoryDTO>(request);
            Clear_History();
            foreach (HistoryDTO history in histories)
            {
                if (history.IsWin)
                {
                    GameObject template = Instantiate(Victory_Prefabs, content.transform);
                    prefabs.Add(template);
                    template.gameObject.GetComponent<History_Prefabs>().SetText(history);
                }
                else
                {
                    GameObject template = Instantiate(Lose_Prefabs, content.transform);
                    prefabs.Add(template);
                    template.gameObject.GetComponent<History_Prefabs>().SetText(history);
                }
            }
        }));

    }

    public void open()
    {
        Historypanel.SetActive(true);
    }
    public void close()
    {
        Historypanel.SetActive(false);
    }

    public void Clear_History()
    {
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }
    }
}