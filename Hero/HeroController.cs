using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    HeroInventory heroInventory;


    public float speed;
    public float maxSpeed;
    public float distance;

    public Vector3 target;
    public Transform targetInteract;

    public string act;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heroInventory = GetComponent<HeroInventory>();
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                ClickUpdate(hit);
            }
        }

        switch(act)
        {
            case "move": Move(); break;
            case "Attack": Attack(); break;
            case "Dealer": Dealer(); break;
            case "": Null(); break;
        }
     
    }

    void ClickUpdate(RaycastHit hit)
    {
        if (hit.transform.tag == "Ground")
        {
            target = hit.point;
            targetInteract = null;
            act = "move";
        }
        else if(hit.transform.tag == "Item")
        {
            targetInteract = null;
            TakeItem(hit);
        }

        else if (hit.transform.tag == "Enemy")
        {
            if (heroInventory.weaponInHand != null)
            {
                targetInteract = hit.transform;
                act = "Attack";
            }
        }
        
        else if(hit.transform.tag == "Dealer")
        {
            targetInteract = hit.transform;
            act = "Dealer";
        }
            
    }

    void Move()
    {
        distance = Vector3.Distance(transform.position, target);
        anim.SetFloat("Speed", speed);
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        if (distance > 0.5f)
        {
           agent.SetDestination(target);
           agent.isStopped = false;
           speed += 2 * Time.deltaTime;
           anim.SetBool("Walk", true);
           anim.SetBool("Attack", false);


           /* if (distance >= 1.2f)
                speed += 20 * Time.deltaTime;
            else if(distance < 1.2f)
                speed -= 20 * Time.deltaTime; */


        }
        else if (distance <= 0.5f)
        {
            speed -= 5 * Time.deltaTime;
            target = transform.position;


            if (speed <= 0.2f)
            {
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", false);
            agent.isStopped = true;
                act = "";
            }
        }
    }

    void Attack()
    {
        distance = Vector3.Distance(transform.position, targetInteract.position);
        anim.SetFloat("Speed", speed);
        speed = Mathf.Clamp(speed, 0, 1);

        if (distance > 2f)
        {
            agent.SetDestination(targetInteract.position);
            agent.isStopped = false;
            speed += 2 * Time.deltaTime;
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);

        }
        else if (distance <= 2f)
        {
            speed -= 5 * Time.deltaTime;

            Vector3 direction = (targetInteract.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

            if (speed <= 0.2f)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", true);
                agent.isStopped = true;
            }
        }
    }
    void Null()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
    }

    void Dealer()
    {
        distance = Vector3.Distance(transform.position, targetInteract.position);
        anim.SetFloat("Speed", speed);
        speed = Mathf.Clamp(speed, 0, 1);

        if (distance > 2f)
        {
            agent.SetDestination(targetInteract.position);
            agent.isStopped = false;
            speed += 2 * Time.deltaTime;
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);

        }
        else
        {
            Dealer dealer = targetInteract.GetComponent<Dealer>();
            dealer.heroInventory = heroInventory;
            heroInventory.Trade = true;
            heroInventory.dealer = dealer;
            dealer.enabled = true;
            act = "";
        }
    }

    void TakeItem(RaycastHit hit)
    {
        distance = Vector3.Distance(transform.position + transform.up, hit.transform.position);
        Item it = hit.transform.GetComponent<Item>();

        if (distance < 2)
        {
            if (heroInventory.weight + it.mass <= heroInventory.maxWeight)
            {
                if (it.typeItem == "Food")
                {
                    heroInventory.food.Add(it);
                    Destroy(hit.transform.gameObject);
                }

                else if (it.typeItem == "Weapon")
                {
                    heroInventory.weapon.Add(it);
                    Destroy(hit.transform.gameObject);
                }
                else if(it.typeItem == "Gold")
                {
                    heroInventory.money += it.price;
                    Destroy(hit.transform.gameObject);
                }
                heroInventory.weight += it.mass;
                heroInventory.sliderWeinght.value = heroInventory.weight;
            }
        }
        else
        {
            print("Далеко");
            print(distance);
        }
    }


    public void Damage()
    {
        targetInteract.GetComponent<NPCStats>().TakeAwayHealth(heroInventory.mainWeapon.item.damage);

        if (targetInteract.GetComponent<NPCStats>().health <= 0)
        {
            act = "";
        }
    }
    
}
