using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
// Move Player base on Dice
public class Step : MonoBehaviour
{
    public int currentPos; // Current Pos of Player in GameBoard list...
    public Player player;



    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public IEnumerator Move(Player player, int step,List<GameObject> race)
    {
        if (GameManager.Instance.isPlayerMoving)
        {
            yield break;
        }
        GameManager.Instance.isPlayerMoving = true;

        while (step > 0)
        {
            currentPos++;
            currentPos %= race.Count;
            Vector3 nextPos = race[this.currentPos].transform.position;
            if (race[this.currentPos].GetComponent<Tile>().Type == TileType.PayCheck)
            {
                Paycheck();
            }
            nextPos.y = 0.25f;
            while (MoveToNextTiles(nextPos, player)) { yield return null; }
            yield return new WaitForSeconds(.2f);
            step--;
        }
        GameManager.Instance.isPlayerMoving = false;


        if (isDreamTile(race[this.currentPos].GetComponent<Tile>()))
        {
            GameManager.Instance.EndGame = true;
        }
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

    public IEnumerator MoveFatRace(Player player)
    {
        if (GameManager.Instance.isPlayerMoving)
        {
            yield break;
        }
        GameManager.Instance.isPlayerMoving = true;
        //currentPos++;
        currentPos %= GameBoard.Instance.Tiles_Fat_Race.Count;
        Vector3 nextPos = GameBoard.Instance.Tiles_Fat_Race[0].transform.position;
        nextPos.y = 0.25f;
        while (MoveToNextTiles(nextPos, player)) { yield return null; }
        yield return new WaitForSeconds(.2f);
            
        GameManager.Instance.isPlayerMoving = false;

        // popup panel when player stop moving
    }

    bool MoveToNextTiles(Vector3 nextTiles,Player player)
    {

        Vector3 newPos = player.Avatar.transform.position; // Store the current position of the game object
        newPos.y += Mathf.Sin(Time.deltaTime * .5f * Mathf.PI) * .5f; // Calculate the new y-coordinate using a sine wave
        //player.Avatar.transform.position = Vector3.MoveTowards(newPos, nextTiles, Time.deltaTime); // Move the game object towards the next position using MoveTowards()
        //return Vector3.Distance(player.Avatar.transform.position, nextTiles) > Mathf.Epsilon;
        return nextTiles != (player.Avatar.transform.position = Vector3.MoveTowards(newPos, nextTiles, 2f * Time.deltaTime));
    }

    void PopupPanel(Tile tile)
    {

        UI_Manager ui = GetComponentInChildren<UI_Manager>();
        // Compare cuurent position to popup panel 
        // Oppotunity, Charity, PayCheck, Offer,DownSize,Doodads
        switch (tile.Type)
        {
            case TileType.Oppotunity:
                UI_Manager.instance.PopUpDeal_UI();
                break;
            case TileType.Market:
                //UI_Manager.Instance.PopUpDeal_UI();
                Market market =  EvenCard_Data.instance.Markets[Random.Range(0, EvenCard_Data.instance.Markets.Count-1)];
                UI_Manager.instance.Popup_Market_Panel(market);
                break;
            case TileType.Baby:
                Baby();
                break;
            case TileType.Charity:
                //UI_Manager.Instance.PopUpDeal_UI();
                Charity();
                break;
            case TileType.DownSize:
                //UI_Manager.Instance.PopUpDeal_UI();
                DownSize();
                break;
            case TileType.Doodads:
                //UI_Manager.Instance.PopUpDeal_UI();
                Doodad doodad = EvenCard_Data.instance.Doodads[Random.Range(0, EvenCard_Data.instance.Doodads.Count-1)];
                UI_Manager.instance.Popup_Doodad_Panel(doodad);
                Doodads(doodad);
                break;
            default:
                break;
        }
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
