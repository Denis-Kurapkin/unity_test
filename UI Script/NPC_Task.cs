using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Task : MonoBehaviour
{
    public bool EndDialog;
    public GameObject Dialog1;
    public GameObject Dialog2;
    public Quest_Event QE;
    public bool Fin_Dialog;
    HeroStats hero;

    void Start()
    {
        
    }

    void Update()
    {
        if (EndDialog == true)
        {
            Time.timeScale = 1;
            QE.Quest1 = true;
            Dialog1.SetActive(false);
        }
        if(Fin_Dialog == true)
        {
            Time.timeScale = 1;
            QE.Quest1 = false;
            Dialog2.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Time.timeScale = 0;
            if (QE.end_Quest1 == false)
            {
                Dialog1.SetActive(true);
            }
            else
            {
                Dialog2.SetActive(true);
            }
        }
    }

}
