using UnityEngine;
using TMPro;


public enum TileType { Oppotunity, Charity, PayCheck, Baby,DownSize,Doodads,Market,CashFlowDay,Divorce, Dream,Accused,Taxes };

public class Tile : MonoBehaviour
{

    public TileType Type;
    //public TextMeshPro titles;
    public Tile_Material default_material;
    public GameObject image;
    [SerializeField] public Dream dream;

    private void Start()
    {
        this.gameObject.transform.localScale *= GameBoard.Instance.size;
        SetMaterialTile(this.Type, default_material);
        dream = new Dream("", "", "", 0);
    }


    public void SetDreamTile(Dream dream)
    {
        this.dream = dream;
    }

    public void SetMaterialTile(TileType type,Tile_Material material)
    {
        switch (type)
        {
            case TileType.Oppotunity:
                this.gameObject.GetComponent<Renderer>().material = material.oppotunity_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.oppotunity_material_img;
                //titles.text = "Oppotunity";
                break;
            case TileType.Market:
                this.gameObject.GetComponent<Renderer>().material = material.offer_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.offer_material_img;
                break;
            case TileType.Charity:
                this.gameObject.GetComponent<Renderer>().material = material.charity_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.charity_material_img;
                break;
            case TileType.CashFlowDay:
            case TileType.PayCheck:
                this.gameObject.GetComponent<Renderer>().material = material.paycheck_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.paycheck_material_img;
                //titles.text = "PayCheck";
                break;
            case TileType.Divorce:
                this.gameObject.GetComponent<Renderer>().material = material.default_material;
                GameObject divorce = Instantiate(material.Divorce);
                divorce.transform.position = new Vector3(this.transform.position.x, divorce.transform.position.y, this.transform.position.z);
                divorce.gameObject.transform.parent = this.transform;
                break;
            case TileType.Baby:
                this.gameObject.GetComponent<Renderer>().material = material.baby_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.baby_material_img;
                //titles.text = "Have Baby";
                break;
            case TileType.Dream:
                foreach (Dream_Item dream in material.dream_Items)
                {
                    Debug.Log(this.dream.id);
                    if (dream.ID == this.dream.id)
                    {
                        GameObject model = Instantiate(dream.Dream_Model);
                        model.transform.position = new Vector3(this.transform.position.x,model.transform.position.y,this.transform.position.z);
                        model.gameObject.transform.parent = this.transform;
                    }
                }
                this.gameObject.GetComponent<Renderer>().material = material.default_material;
                break;
            case TileType.DownSize:
                this.gameObject.GetComponent<Renderer>().material = material.downsize_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.downsize_material_img;
                //titles.text = "DownSize";
                break;
            case TileType.Accused:
                this.gameObject.GetComponent<Renderer>().material = material.default_material;
                GameObject accused = Instantiate(material.Accused);
                accused.transform.position = new Vector3(this.transform.position.x, accused.transform.position.y, this.transform.position.z);
                accused.gameObject.transform.parent = this.transform;
                break;
            case TileType.Taxes:
                this.gameObject.GetComponent<Renderer>().material = material.default_material;
                GameObject taxes = Instantiate(material.Taxes_Model);
                taxes.transform.position = new Vector3(this.transform.position.x, taxes.transform.position.y, this.transform.position.z);
                taxes.gameObject.transform.parent = this.transform;
                break;
            case TileType.Doodads:
                this.gameObject.GetComponent<Renderer>().material = material.doodads_material;
                this.image.gameObject.GetComponent<Renderer>().material = material.doodads_material_img;
                //titles.text = "Doodads";
                break;
            default:
                this.gameObject.GetComponent<Renderer>().material = material.default_material;
                //titles.text = "";
                break;
        }
    }
}
