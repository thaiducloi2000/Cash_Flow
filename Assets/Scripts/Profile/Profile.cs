using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Profile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ProfilePanel;
    public GameObject Job_Image;
    public TMP_Text profilename;
    public TMP_Text Job_Name;
    public void openProfile()
    {
        User_Data user = Resources.Load<User_Data>("Items/User_Data");
        ProfilePanel.SetActive(true);
        profilename.text = user.data.user.NickName;
        Job_Name.text = user.LastJobSelected.Job_card_name;
        Job_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/" + user.LastJobSelected.Image_url);
    }
    public void closeProfile()
    {
        ProfilePanel.SetActive(false);
    }

    public void ChangeJob()
    {
        SceneManager.LoadSceneAsync("CharacterSelection");
    }
}
