using Fusion;
using UnityEngine;
// Manage UI Canvas in Game
public class UI_Manager : NetworkBehaviour
{
    public static UI_Manager instance;
    public GameObject Deal_Panel;
    public GameObject Oppturnity_Panel;
    public GameObject Baby_Panel;
    public GameObject Charity_Panel;
    public GameObject DownSize_Panel;
    public GameObject Dream_Panel;
    public GameObject Select_Deal_Type_Panel;
    public GameObject Job_Panel;
    public GameObject Financial_Panel;
    public GameObject Market_Panel;
    public GameObject Doodad_Panel;
    public GameObject Roll_Button;
    public GameObject Rental_Panel;
    public GameObject ProblemContainer_Panel;
    private int b_deal = -1;
    private int s_deal = -1;
    private int f_oppotunity = -1;
    //[SerializeField] Player player;
    public InfoPlayers infoPlayers;
    public User_Data user;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        UnAcctiveAllPanel();
    }
    public void PopupJob_UI(Job job)
    {
        Job_Panel.GetComponent<Job_Panel>().loadJobInformation(job);
    }

    private void UnAcctiveAllPanel()
    {
        Deal_Panel.SetActive(false);
        Select_Deal_Type_Panel.SetActive(false);
        Doodad_Panel.SetActive(false);
        Market_Panel.SetActive(false);
        Job_Panel.SetActive(true);
    }

    public void PopUpDeal_UI()
    {
        // Open Select Deal Type first
        Select_Deal_Type_Panel.SetActive(true);

    }

    public void Denine_Deal_Btn()
    {
        Deal_Panel.SetActive(false);
        Oppturnity_Panel.SetActive(false);
        Reset_Deal();
    }

    public void Accept_Deal_Btn()
    {
        if (b_deal >= 0)
        {
            //player.MoveToFatRace();
            if (Player.Instance.financial_rp.GetCash() < EvenCard_Data.instance.Big_Deal_List[b_deal].Cost && Player.Instance.financial_rp.GetCash() < EvenCard_Data.instance.Big_Deal_List[b_deal].Downpay)
            {
                Rental_Panel rent_panel = this.Rental_Panel.GetComponent<Rental_Panel>();
                rent_panel.Show_Penel();
            }
            else
            {
                ApplyBigDeal(EvenCard_Data.instance.Big_Deal_List[b_deal]);
                if (Player.Instance.financial_rp.GetPassiveIncome()>= 0 && Player.Instance.isInFatRace == false)
                {

                    Player.Instance.isInFatRace = Player.Instance.financial_rp.GetPassiveIncome() >= 0;
                    Player.Instance.MoveToFatRace();
                }
                Deal_Panel.SetActive(false);
                Reset_Deal();
            }
        }
        else if (s_deal >= 0)
        {
            if (Player.Instance.financial_rp.GetCash() < (EvenCard_Data.instance.Small_Deal_List[s_deal].Cost * 100) && Player.Instance.financial_rp.GetCash() < EvenCard_Data.instance.Small_Deal_List[s_deal].Downsize * 100)
            {
                Rental_Panel rent_panel = this.Rental_Panel.GetComponent<Rental_Panel>();
                rent_panel.Show_Penel();
            }
            else
            {
                ApplySmallDeal(EvenCard_Data.instance.Small_Deal_List[s_deal], 100);
                if (Player.Instance.financial_rp.GetPassiveIncome() >= 0 && Player.Instance.isInFatRace == false)
                {

                    Player.Instance.isInFatRace = Player.Instance.financial_rp.GetPassiveIncome() >= 0;
                    Player.Instance.MoveToFatRace();
                }
                Deal_Panel.SetActive(false);
                Reset_Deal();
            }
        }
        //Player.Instance.GetComponent<Player>().UpdatePlayerTurn();
        UpdateProfilePlayer();
        //GameManager.Instance.RPC_nextTurn();
    }

    public void PopupDownSize()
    {
        DownSize_Panel.SetActive(true);
    }
    public void PopupCharity()
    {
        Charity_Panel.SetActive(true);
    }

    public void CloseDownSize()
    {
        DownSize_Panel.SetActive(false);
    }

    public void CloseCharity()
    {
        Charity_Panel.SetActive(false);
    }


    public void Popup_Dream_Panel(Tile tile)
    {
        Dream_Panel.SetActive(true);
        Dream_Panel.GetComponent<Dream_Panel>().SetDream(tile);
    }
    public void Accept_Oppotunity_Btn()
    {
        if (f_oppotunity >= 0)
        {
            if (Player.Instance.financial_rp_fat_race.GetCash() < EvenCard_Data.instance.fat_race_oppoturnities[f_oppotunity].Cost && Player.Instance.financial_rp_fat_race.GetCash() < EvenCard_Data.instance.fat_race_oppoturnities[f_oppotunity].Downpay)
            {
                Rental_Panel rent_panel = this.Rental_Panel.GetComponent<Rental_Panel>();
                rent_panel.Show_Penel();
            }
            else
            {
                ApplyBigDeal_FatRace(EvenCard_Data.instance.fat_race_oppoturnities[f_oppotunity]);
                if (Player.Instance.financial_rp_fat_race.GetPassiveIncome()-Player.Instance.financial_rp.GetPassiveIncome()*100 >= 50000)
                {
                    GameManager.Instance.EndGame = true;
                    Player.Instance.GetComponent<Player>().result = true;
                    GameManager.Instance.CheckPlayerWinner();
                }
                Oppturnity_Panel.SetActive(false);
                Reset_Deal();
            }
        }
        //Player.Instance.GetComponent<Player>().UpdatePlayerTurn();
        UpdateProfilePlayer();
        //GameManager.Instance.RPC_nextTurn();
    }

    private void ApplyBigDeal_FatRace(Big_Deal deal)
    {
        switch (deal.Action)
        {
            case 1:
                if (deal.Downpay > 0)
                {
                    Player.Instance.financial_rp_fat_race.SetCash(Player.Instance.financial_rp_fat_race.GetCash() - deal.Downpay);
                }
                else if (deal.Downpay == deal.Cost)
                {
                    Player.Instance.financial_rp_fat_race.SetCash(Player.Instance.financial_rp_fat_race.GetCash() - deal.Downpay);
                }
                else
                {
                    Player.Instance.financial_rp_fat_race.SetCash(Player.Instance.financial_rp_fat_race.GetCash() - deal.Cost);
                }
                // Add Game Account
                if (deal.Cash_flow == 0)
                {
                    Asset game_Accounts = new Asset(deal.Account_Name, deal.Cost, 1);
                    Player.Instance.financial_rp_fat_race.game_accounts.Add(game_Accounts);
                }
                else if (deal.Cash_flow > 0 && deal.Downpay < deal.Cost)
                {
                    Income income = new Income(deal.Account_Name, deal.Cash_flow, 1);
                    Liability lia = new Liability(deal.Account_Name, deal.Dept, 1);
                    Asset asset = new Asset(deal.Account_Name, deal.Cost, 1);
                    Player.Instance.financial_rp_fat_race.game_accounts.Add(income);
                    Player.Instance.financial_rp_fat_race.game_accounts.Add(lia);
                    Player.Instance.financial_rp_fat_race.game_accounts.Add(asset);
                }
                break;
            case 3:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp_fat_race.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        Player.Instance.financial_rp_fat_race.SetCash(Player.Instance.financial_rp_fat_race.GetCash() - deal.Cost);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void PopupBabyUI()
    {
        Baby_Panel.SetActive(true);
    }

    public void CloseBabyUI()
    {
        Baby_Panel.SetActive(false);
    }

    public void Popup_Market_Panel(Market market)
    {
        //if (market.Image != null)
        //{
        //    StartCoroutine(EvenCard_Data.instance.helper.DownloadImage(market.Image, (Sprite) => {
        Market_Panel.SetActive(true);

        Market_Panel panel = Market_Panel.GetComponent<Market_Panel>();
       // panel.SetMarketPanel(market, Sprite);
        panel.SetMarketPanel(market);
        //    }));
        //}
    }

    public void Popup_Doodad_Panel(Doodad doodad)
    {
        //if (doodad.Image != null)
        //{
        //    StartCoroutine(EvenCard_Data.instance.helper.DownloadImage(doodad.Image, (Sprite) => {
        Doodad_Panel.SetActive(true);

        Doodad_Panel panel = Doodad_Panel.GetComponent<Doodad_Panel>();
        //panel.SetDoodadPanel(doodad, Sprite);
        panel.SetDoodadPanel(doodad);
        //    }));
        //}
    }


    public void Big_Deal_Btn()
    {
        Select_Deal_Type_Panel.SetActive(false);
        int deal_num = Random.Range(0, EvenCard_Data.instance.Big_Deal_List.Count - 1);
        Big_Deal deal = EvenCard_Data.instance.Big_Deal_List[deal_num];
        //if (deal.Image != null)
        //{
        //    StartCoroutine(EvenCard_Data.instance.helper.DownloadImage(deal.Image, (Sprite) => {
        Deal_Panel.SetActive(true);

        // Get BIg Deal From Instance
        Deal_Panel panel = Deal_Panel.GetComponent<Deal_Panel>();
        //panel.SetBigDeal(deal, Sprite);
        panel.SetBigDeal(deal);
        //    }));
        //}
        b_deal = deal_num;
    }

    private void ApplyBigDeal(Big_Deal deal)
    {
        switch (deal.Action)
        {
            case 1:
                if (deal.Downpay > 0)
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Downpay);
                }
                else if (deal.Downpay == deal.Cost)
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Downpay);
                }
                else
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Cost);
                }
                // Add Game Account
                if (deal.Cash_flow == 0)
                {
                    Asset game_Accounts = new Asset(deal.Account_Name, deal.Cost, 1);
                    Player.Instance.financial_rp.game_accounts.Add(game_Accounts);
                }
                else if (deal.Cash_flow > 0 && deal.Downpay < deal.Cost)
                {
                    Income income = new Income(deal.Account_Name, deal.Cash_flow, 1);
                    Liability lia = new Liability(deal.Account_Name, deal.Dept, 1);
                    Asset asset = new Asset(deal.Account_Name, deal.Cost, 1);
                    Player.Instance.financial_rp.game_accounts.Add(income);
                    Player.Instance.financial_rp.game_accounts.Add(lia);
                    Player.Instance.financial_rp.game_accounts.Add(asset);
                }
                break;
            case 3:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Cost);
                    }
                }
                break;
            case 4:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        game_accounts.Game_account_value /= 2;
                    }
                }
                break;
            case 5:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        game_accounts.Game_account_value *= 2;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Fat_Race_Oppotunity()
    {
        int deal_num = Random.Range(0, EvenCard_Data.instance.fat_race_oppoturnities.Count - 1);
        Big_Deal deal = EvenCard_Data.instance.fat_race_oppoturnities[deal_num];
        //if (deal.Image != null)
        //{
        //    StartCoroutine(EvenCard_Data.instance.helper.DownloadImage(deal.Image, (Sprite) => {
        Oppturnity_Panel.SetActive(true);

        // Get BIg Deal From Instance
        Debug.Log(deal.Title);
        Oppturnity_Panel panel = Oppturnity_Panel.GetComponent<Oppturnity_Panel>();
        //panel.SetBigDeal(deal, Sprite);
        panel.SetBigDeal(deal);
        //    }));
        //}
        f_oppotunity = deal_num;
    }
    public void Roll_Dice()
    {
        //if (GameManager.Instance.EndGame != true)
        //{
        //    // Dress Key button to roll
        //    // Role Dice
        //    int step = Random.Range(1, 7);
        //    // Move
        //    if (Player.Instance.isInFatRace)
        //    {
        //        StartCoroutine(Player.Instance.gameObject.GetComponent<Step>().Move(Player.Instance, step, GameBoard.Instance.Tiles_Fat_Race));
        //    }
        //    else
        //    {
        //        StartCoroutine(Player.Instance.gameObject.GetComponent<Step>().Move(Player.Instance, step, GameBoard.Instance.Tiles_Rat_Race));
        //    }

        //    GameManager.Instance.nextTurn();
        //}
        Player.Instance.GetComponent<Player>().OnRollDice();
    }

    public void AcceptJob()
    {
        Player.Instance.user_data.LastJobSelected = UI_Manager.instance.Job_Panel.GetComponent<Job_Panel>().job;
        UI_Manager.instance.Job_Panel.SetActive(false);
        ApplyJobToFinancial(Player.Instance.user_data.LastJobSelected);
        UI_Manager.instance.Financial_Panel.GetComponent<Financial_Panel_Manager>().Financial(Player.Instance.financial_rp);
        UpdateProfilePlayer();
    }

    public void ApplyJobToFinancial(Job job)
    {
        float expense = 0;
        foreach (Game_accounts account in job.Game_accounts)
        {
            if (account.Game_account_type == AccountType.Expense)
            {
                expense += account.Game_account_value;
            }
        }
        Player.Instance.Children_cost = job.Children_cost;
        Player.Instance.financial_rp = new Financial(0, Player.Instance.id, job.Job_card_name, 0, expense, job.Game_accounts);
        Player.Instance.dreams = GameBoard.Instance.GetListDream();
    }

    private void ApplySmallDeal(Small_Deal deal, int amount)
    {
        switch (deal.Action)
        {
            case 1:
                if (deal.Downsize > 0)
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Downsize * amount);
                }
                else if (deal.Downsize == deal.Cost)
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Downsize * amount);
                }
                else
                {
                    Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Cost * amount);
                }
                // Add Game Account
                if (deal.Cash_flow == 0)
                {
                    Asset game_Accounts = new Asset(deal.Account_Name, deal.Cost, amount);
                    Player.Instance.financial_rp.game_accounts.Add(game_Accounts);
                }
                else if (deal.Cash_flow > 0 && deal.Downsize < deal.Cost)
                {
                    Income income = new Income(deal.Account_Name, deal.Cash_flow, amount);
                    Liability lia = new Liability(deal.Account_Name, deal.Dept, amount);
                    Asset asset = new Asset(deal.Account_Name, deal.Cost, amount);
                    Player.Instance.financial_rp.game_accounts.Add(income);
                    Player.Instance.financial_rp.game_accounts.Add(lia);
                    Player.Instance.financial_rp.game_accounts.Add(asset);
                }
                break;
            case 3:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - deal.Cost);
                    }
                }
                break;
            case 4:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        game_accounts.Game_account_value /= 2;
                    }
                }
                break;
            case 5:
                foreach (Game_accounts game_accounts in Player.Instance.financial_rp.game_accounts)
                {
                    if (game_accounts.Game_account_name.Contains(deal.Account_Name))
                    {
                        game_accounts.Game_account_value *= 2;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Small_Deal_Btn()
    {
        Select_Deal_Type_Panel.SetActive(false);
        
        int deal_num = Random.Range(0, EvenCard_Data.instance.Small_Deal_List.Count - 1);
        Small_Deal deal = EvenCard_Data.instance.Small_Deal_List[deal_num];
        if (deal.Image != null)
        {
            StartCoroutine(EvenCard_Data.instance.helper.DownloadImage(deal.Image, (Sprite) => {
                Deal_Panel.SetActive(true);


                // Get Small Deal From Instance
                Deal_Panel panel = Deal_Panel.GetComponent<Deal_Panel>();
                panel.SetSmallDeal(deal, Sprite);
            }));
        }
        s_deal = deal_num;

    }

    private void Reset_Deal()
    {
        this.s_deal = -1;
        this.b_deal = -1;
        this.f_oppotunity = -1;
    }

    public void UpdateProfilePlayer()
    {
        infoPlayers.SetInfoPlayers(Player.Instance.infoNumber, 
            user.data.user.NickName, 
            user.LastJobSelected.Job_card_name, 
            Player.Instance.financial_rp.GetCash(), 
            Player.Instance.financial_rp.children_amount);
    }

}
