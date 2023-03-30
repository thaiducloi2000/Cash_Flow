using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InventoryPanel;
    public GameObject ChessPage;
    public GameObject ShirtPage;
    public GameObject PantPage;
    public GameObject ShoePage;
    public GameObject HairPage;
    public void openInven()
    {
        InventoryPanel.SetActive(true);
    }
    public void closeInven()
    {
        InventoryPanel.SetActive(false);
    }

    public void OpenChessPage()
    {
        ChessPage.SetActive(true);
        ShirtPage.SetActive(false);
        PantPage.SetActive(false);
        ShoePage.SetActive(false);
        HairPage.SetActive(false);
    }
    public void OpenShirtPage()
    {
        ChessPage.SetActive(false);
        ShirtPage.SetActive(true);
        PantPage.SetActive(false);
        ShoePage.SetActive(false);
        HairPage.SetActive(false);
    }
    public void OpenPantPage()
    {
        ChessPage.SetActive(false);
        ShirtPage.SetActive(false);
        PantPage.SetActive(true);
        ShoePage.SetActive(false);
        HairPage.SetActive(false);
    }
    public void OpenShoePage()
    {
        ChessPage.SetActive(false);
        ShirtPage.SetActive(false);
        PantPage.SetActive(false);
        ShoePage.SetActive(true);
        HairPage.SetActive(false);
    }
    public void OpenHairPage()
    {
        ChessPage.SetActive(false);
        ShirtPage.SetActive(false);
        PantPage.SetActive(false);
        ShoePage.SetActive(false);
        HairPage.SetActive(true);
    }


}
