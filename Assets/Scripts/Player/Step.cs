using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
// Move Player base on Dice
public enum BoardType { RatRace, FatRace }

public class Step : NetworkBehaviour
{
    [Networked]
    public int currentPos { get; set; } // Current Pos of Player in GameBoard list...
    public Player player;
    [Networked]
    private NetworkBool moveToNextTile { get; set; }
    public List<GameObject> Ratrace;
    public List<GameObject> Fatrace;
    public List<GameObject> race;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public override void Spawned()
    {
        Ratrace = GameBoard.Instance.Tiles_Rat_Race;
        Fatrace = GameBoard.Instance.Tiles_Fat_Race;
    }

    public IEnumerator Move(Player player, int step, BoardType boardType)
    {
        yield return new WaitForSeconds(1.05f);
        GetComponent<Player>().RPC_HideDice(false);
        //DanhNPC add check board race
        if (boardType == BoardType.RatRace)
            race = Ratrace;
        else if (boardType == BoardType.FatRace)
            race = Fatrace;
        
        if (GameManager.Instance.isPlayerMoving)
        {
            yield break;
        }
        GameManager.Instance.isPlayerMoving = true;
        while (step > 0)
        {
            RPC_Moving(boardType);
            if (this.moveToNextTile) { yield return null; }
            yield return new WaitForSeconds(.5f);
            step--;
        }
        
        GameManager.Instance.isPlayerMoving = false;


        // popup panel when player stop moving 
        PopupPanel(race[this.currentPos].GetComponent<Tile>());
    }

    private bool isDreamTile(Tile tile)
    {
        foreach (Dream dream in player.dreams)
        {
            if (dream.id == tile.dream.id)
            {
                return true;
            }
        }
        return false;
    }

    //DanhNPC add online move function
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_MovingToFatRace(RpcInfo info = default)
    {
        currentPos %= GameBoard.Instance.Tiles_Fat_Race.Count;
        Vector3 nextPos = GameBoard.Instance.Tiles_Fat_Race[0].transform.position;
        nextPos.y = 0.05f;
        MoveToNextTiles(nextPos);
    }

    public IEnumerator MoveFatRace(Player player)
    {
        if (GameManager.Instance.isPlayerMoving)
        {
            yield break;
        }
        GameManager.Instance.isPlayerMoving = true;
        int count = 4;
        //currentPos++;
        //currentPos %= GameBoard.Instance.Tiles_Fat_Race.Count;
        //Vector3 nextPos = GameBoard.Instance.Tiles_Fat_Race[0].transform.position;
        //nextPos.y = 0.05f;
        //MoveToNextTiles(nextPos);

        while (count > 0)
        {
            RPC_MovingToFatRace();
            if (this.moveToNextTile) { yield return null; }
            yield return new WaitForSeconds(.2f);
            count--;
        }
        GetComponent<Player>().RPC_HideDice(false);
        GameManager.Instance.isPlayerMoving = false;

        // popup panel when player stop moving
    }

    //DanhNPC add online move function
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_Moving(BoardType boardType , RpcInfo info = default)
    {
        //DanhNPC add check board race
        if (boardType == BoardType.RatRace)
            race = Ratrace;
        else if (boardType == BoardType.FatRace)
            race = Fatrace;
        currentPos++;
        currentPos %= race.Count;
        Vector3 nextPos = race[this.currentPos].transform.position;
        if (race[this.currentPos].GetComponent<Tile>().Type == TileType.PayCheck)
        {
            if (Object.HasInputAuthority)
                Paycheck();
        }
        nextPos.y = 0.05f;
        MoveToNextTiles(nextPos);

    }
    //DanhNPC modified move function
    void MoveToNextTiles(Vector3 nextTiles)
    {
        float speed = 2f;
        float rotationSpeed = 10f;

        Vector3 newPos = transform.position;
        newPos.y += Mathf.Sin(Time.deltaTime * .5f * Mathf.PI) * .5f;

        Vector3 targetDirection = nextTiles - transform.position;
        targetDirection.y = 0; // Set the Y component of the direction to 0
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);

        // Move the player towards the nextTiles position
        transform.position = Vector3.MoveTowards(newPos, nextTiles, speed );
        //RPC_Moving(nextTiles);

        // Return true if the player has reached the nextTiles position
        //this.moveToNextTile = transform.position != nextTiles;
        this.moveToNextTile = transform.position != nextTiles; //changed return bool to void
    }

    void PopupPanel(Tile tile)
    {
        // Compare cuurent position to popup panel 
        // Oppotunity, Charity, PayCheck, Offer,DownSize,Doodads
        switch (tile.Type)
        {
            case TileType.Oppotunity:
                if (Player.Instance.isInFatRace)
                {
                    UI_Manager.instance.Fat_Race_Oppotunity();
                }
                else
                {
                    UI_Manager.instance.PopUpDeal_UI();
                }
                break;
            case TileType.Market:
                //UI_Manager.Instance.PopUpDeal_UI();
                Market market =  EvenCard_Data.instance.Markets[Random.Range(0, EvenCard_Data.instance.Markets.Count-1)];
                UI_Manager.instance.Popup_Market_Panel(market);
                break;
            case TileType.Baby:
                Baby();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
               
                break;
            case TileType.Charity:
                //UI_Manager.Instance.PopUpDeal_UI();
                Charity();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                break;
            case TileType.DownSize:
                //UI_Manager.Instance.PopUpDeal_UI();
                DownSize();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                break;
            case TileType.Doodads:
                //UI_Manager.Instance.PopUpDeal_UI();
                Doodad doodad = EvenCard_Data.instance.Doodads[Random.Range(0, EvenCard_Data.instance.Doodads.Count-1)];
                UI_Manager.instance.Popup_Doodad_Panel(doodad);
                Doodads(doodad);
                break;
            case TileType.Divorce:
                Divorce();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                Debug.Log("Divorce");
                break;
            case TileType.CashFlowDay:
                CashFlowDay();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                Debug.Log("CashFlowDay");
                break;
            case TileType.Accused:
                Accused();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                Debug.Log("Accused");
                break;
            case TileType.Taxes:
                Taxes();
                //player.UpdatePlayerTurn();
                UI_Manager.instance.UpdateProfilePlayer();
                
                Debug.Log("Taxes");
                break;
            case TileType.Dream:
                Dream(currentPos);
                //player.UpdatePlayerTurn();
                
                break;
            case TileType.PayCheck:
                //player.UpdatePlayerTurn();
                
                break;
            default:
                break;
        }
    }

    private void Dream(int pos)
    {
        if (isDreamTile(GameBoard.Instance.Tiles_Fat_Race[pos].GetComponent<Tile>()))
        {
            GameManager.Instance.EndGame = true;
            Player.Instance.GetComponent<Player>().result = true;
            GameManager.Instance.CheckPlayerWinner();
        }
    }

    private void Divorce()
    {
        // -50% cash
        UI_Manager.instance.ProblemContainer_Panel.GetComponent<ProblemContainer_Panel>().Popup_Divorce_Panel();
        player.financial_rp_fat_race.SetCash(player.financial_rp_fat_race.GetCash() - (player.financial_rp_fat_race.GetCash() / 2));
    }

    private void CashFlowDay()
    {
        // get cash
        float total_income = 0;
        foreach (Game_accounts account in player.financial_rp_fat_race.game_accounts)
        {
            if (account.Game_account_type == AccountType.Income)
            {
                total_income += account.Game_account_value;
            }
        }
        player.financial_rp_fat_race.SetCash(player.financial_rp_fat_race.GetCash() + total_income);
    }

    private void Accused()
    {
        // -25%
        UI_Manager.instance.ProblemContainer_Panel.GetComponent<ProblemContainer_Panel>().Popup_Litigation_Panel();
        player.financial_rp_fat_race.SetCash(player.financial_rp_fat_race.GetCash() - (player.financial_rp_fat_race.GetCash() / 4));
    }

    private void Taxes()
    {
        // -10%
        UI_Manager.instance.ProblemContainer_Panel.GetComponent<ProblemContainer_Panel>().Popup_Taxes_Panel();
        player.financial_rp_fat_race.SetCash(player.financial_rp_fat_race.GetCash() - (player.financial_rp_fat_race.GetCash() / 10));
    }

    private void Paycheck()
    {
        float total_expense = 0;
        float total_income = 0;
        foreach(Game_accounts account in player.financial_rp.game_accounts)
        {
            if(account.Game_account_type == AccountType.Income)
            {
                total_income += account.Game_account_value;
            }else if(account.Game_account_type == AccountType.Expense)
            {
                total_expense += account.Game_account_value;
            }
        }
        player.financial_rp.SetCash(player.financial_rp.GetCash() + total_income - total_expense);
        UI_Manager.instance.UpdateProfilePlayer();
    }

    private void Doodads(Doodad doodad)
    {
        player.financial_rp.SetCash(player.financial_rp.GetCash() - doodad.Cost);
    }

    private void DownSize()
    {
        float total_expense = 0;
        foreach (Game_accounts account in player.financial_rp.game_accounts)
        {
            if (account.Game_account_type == AccountType.Expense)
            {
                total_expense += account.Game_account_value;
            }
        }
        player.financial_rp.SetCash(player.financial_rp.GetCash() + - total_expense);
        // Add Code to missing 2 turn

    }

    private void Charity()
    {
        float total_income = 0;
        foreach (Game_accounts account in player.financial_rp.game_accounts)
        {
            if (account.Game_account_type == AccountType.Income)
            {
                total_income += account.Game_account_value;
            }
        }
        player.financial_rp.SetCash(player.financial_rp.GetCash() - (total_income/10));
    }

    private void Baby()
    {
        UI_Manager.instance.PopupBabyUI();
        if(player.financial_rp.children_amount < 3)
        {
            player.financial_rp.children_amount++;
        }
        foreach(Game_accounts account in player.financial_rp.game_accounts)
        {
            if(account.Game_account_name == "Child rearing costs")
            {
                account.Game_account_value = player.financial_rp.children_amount * player.Children_cost;
            }
        }
    }
}
