using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPlayer : NetworkBehaviour
{
    public Image image;
    public TMP_Text name;
    public TMP_Text job;
    public TMP_Text money;
    public TMP_Text baby;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetInformation(string name, string job, float money, int baby, RpcInfo info = default)
    {
        this.name.text = name;
        this.job.text = "(" + job +")";
        this.money.text = money.ToString();
        this.baby.text = baby.ToString();
    }
    
}
