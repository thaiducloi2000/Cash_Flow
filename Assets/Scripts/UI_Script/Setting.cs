using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Setting : MonoBehaviour
{
    public GameObject Settingpanel;
    public AudioMixer audiomixer;
    public GameObject AttentionExitpanel;
    // Start is called before the first frame update

    public void openSetting()
    {
        Settingpanel.SetActive(true);
    }

    public void closeSetting()
    {
        Settingpanel.SetActive(false);
    }
    public void SetMusic(float music)
    {
        audiomixer.SetFloat("music",music);
    }
    public void showpopupexit()
    {
        AttentionExitpanel.SetActive(true);
    }
    public void closeexitattention()
    {
        AttentionExitpanel.SetActive(false);
    }
}
