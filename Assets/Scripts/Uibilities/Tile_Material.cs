using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Item_Type {Tile,Board,Avatar,Chess,Dice }

[CreateAssetMenu(fileName = "Tile_material", menuName = "Tile/Material", order = 1)]
public class Tile_Material : ScriptableObject
{
    public Item_Type item;
    public string id;
    [Header("Color Material")]
    public Material oppotunity_material;
    public Material charity_material;
    public Material paycheck_material;
    public Material offer_material;
    public Material downsize_material;
    public Material doodads_material;
    public Material baby_material;
    public Material default_material;
    [Header("Image Material")]
    public Material oppotunity_material_img;
    public Material charity_material_img;
    public Material paycheck_material_img;
    public Material offer_material_img;
    public Material downsize_material_img;
    public Material doodads_material_img;
    public Material baby_material_img;
}
