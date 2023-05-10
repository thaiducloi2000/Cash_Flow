using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Cecil;

public class SwipeSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scroll_bar;
    public TMP_Text job_name;
    public TMP_Text job_salary;
    public TMP_Text job_cash;
    public TMP_Text job_tax;
    //public Image image;
    public Color color;
    public float scroll_pos = 0;
    public float[] pos;
    public float current_pos;
    // Prefabs Pooling Object
    public GameObject template;

    public Job job_Selected;

    public List<GameObject> pool_item;
    public Game_Data data;

    private void Awake()
    {
        GenarateItem();
        
    }

    void Update()
    {
        Swipe();
    }


    public void GenarateItem()
    {
        pool_item = new List<GameObject>();
        // change ti Item when have full Item;
        foreach (Job job in data.jobs)
        {
            var image = Resources.Load<Sprite>("Sprite/"+job.Image_url);
            GameObject item = Instantiate(template, this.transform);
            item.GetComponent<Image>().sprite = image;
            pool_item.Add(item);
        }
    }

    public void Swipe()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scroll_bar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scroll_bar.GetComponent<Scrollbar>().value = Mathf.Lerp(scroll_bar.GetComponent<Scrollbar>().value, pos[i], 0.2f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.1f, 1.1f), 0.2f);
                color.a = 1;
                job_name.text = data.jobs[i].Job_card_name;
                job_salary.text = data.jobs[i].GetSalary().ToString();
                job_cash.text = data.jobs[i].GetCash().ToString();
                job_tax.text = data.jobs[i].GetTax().ToString();
                this.job_Selected = data.jobs[i];
                transform.GetChild(i).GetComponent<Image>().color = color;

                //jobname.text = jobnames.text;
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.7f, 0.7f), 0.2f);
                        color.a = 0.4f;
                        transform.GetChild(a).GetComponent<Image>().color = color;
                    }
                }

            }
        }
    }
}
