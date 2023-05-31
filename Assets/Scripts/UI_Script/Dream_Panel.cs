using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dream_Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private Image dream_image;
    public Dream dream;

    public void SetDream(Tile tile)
    {
        this.dream = tile.dream;
        this.Price.text = dream.Cost.ToString();
        foreach (Dream_Item dream in tile.default_material.dream_Items)
        {
            if (dream.ID == this.dream.id)
            {
                this.dream_image.sprite = dream.Dream_Sprite;
            }
        }
    }

    public void AcceptDream()
    {
        this.gameObject.SetActive(false);
        Player.Instance.financial_rp_fat_race.SetCash(Player.Instance.financial_rp_fat_race.GetCash() - dream.Cost);
        foreach (Dream dream in Player.Instance.dreams)
        {
            if (this.dream.id == dream.id)
            {
                GameManager.Instance.EndGame = true;
                Player.Instance.GetComponent<Player>().result = true;
                GameManager.Instance.CheckPlayerWinner();
            }
        }
    }

    public void DenineDream()
    {
        this.gameObject.SetActive(false);
    }
}
