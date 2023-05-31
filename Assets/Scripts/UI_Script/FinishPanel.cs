using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPanel : NetworkBehaviour
{
    public static FinishPanel instance;
    public GameObject WinPanel;
    public GameObject LosePanel;

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
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        LosePanel.SetActive(false);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_Lose(RpcInfo info = default)
    {
        LosePanel.SetActive(true);
    }

    public void OK_Btn()
    {
        Player.Instance.Save();
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public void BackToShop()
    {
        SceneManager.LoadSceneAsync("TestShopScene");
    }
    
}
