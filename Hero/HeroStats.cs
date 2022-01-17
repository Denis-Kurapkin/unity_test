using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : MonoBehaviour
{
    public float health;
    public float maxHealth;


    public Image imgHealth;

    public int level;
    public float exp;
    public float curExp;
    public Image imgExp;
    Animator anim;

    public Transform[] element;

    public int strenght;
    public float multiplierStrenght;

    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        exp = 100 * level;
        InterfaceUpdate();
        CalculateStats();
    }

    private void Update()
    {
        //health += Time.deltaTime / 3;
        InterfaceUpdate();
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void AddHealth(int add)
    {
        health += add;

        health = Mathf.Clamp(health, 0, maxHealth);
        InterfaceUpdate();
    }


    public void AddExp(int add)
    {
        curExp += add;

        if(curExp == exp)
        {
            level++;
            exp = 100 * level;
            curExp = 0;
            strenght += 3;
            CalculateStats();
        }
        else if(curExp > exp)
        {
            curExp -= exp;
            level++;
            exp = 100 * level;
            strenght += 3;
            CalculateStats();
        }
        InterfaceUpdate();
    }

    public void TakeAwayHealth(int takeAway)
    {
        health -= takeAway;

        if (health <= 0)
        {
            col.enabled = false;
        }
    }

    public void InterfaceUpdate()
    {
        float timeHealth = health / maxHealth * 100;
        imgHealth.fillAmount = timeHealth / 100;

        float timeExp = curExp / exp * 100;
        imgExp.fillAmount = timeExp / 100;

    }

    public void CalculateStats()
    {
        maxHealth = strenght * multiplierStrenght;
    }



}
