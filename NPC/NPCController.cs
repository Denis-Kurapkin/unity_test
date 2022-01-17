using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public List<Transform> wayPoint;
    public int curWayPoint;

    public Transform fishingPosition;

    Animator anim;
    public float speed;

    NavMeshAgent agent;


    public bool fishing;


    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (!fishing)
            Patrol();
        else Fishing();
    }

    void Patrol()
    {
        if(wayPoint.Count > 1)
        {
            if(wayPoint.Count > curWayPoint)
            {
                agent.SetDestination(wayPoint[curWayPoint].position);
                float distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);

                if(distance > 2.5f)
                {
                    anim.SetFloat("Speed", speed);
                    speed += Time.deltaTime * 3;
                }
                else if (distance <= 2.5f && distance >= 1f)
                {
                    Vector3 direction = (wayPoint[curWayPoint].position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
                }
                else
                {
                    curWayPoint++;
                }

            }
            else if(wayPoint.Count == curWayPoint)
            {
                curWayPoint = 0;
            }

        }
        else if(wayPoint.Count == 1)
        {
            agent.SetDestination(wayPoint[0].position);
            float distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);
            if (distance > 1.5f)
            {
                agent.isStopped = false;
                anim.SetFloat("Speed", speed);
                speed += Time.deltaTime * 3;
            }
            else
            {
                agent.isStopped = true;
                anim.SetFloat("Speed", speed);
                speed -= Time.deltaTime * 5;

                Vector3 direction = (wayPoint[0].position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            }
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", speed);
            speed -= Time.deltaTime * 5;
        }
        anim.SetBool("Fishing", false);
        speed = Mathf.Clamp(speed, 0, 1.5f);

    }

    void Fishing()
    {
        agent.SetDestination(fishingPosition.position);
        float distance = Vector3.Distance(transform.position, fishingPosition.position);
        if (distance > 1.5f)
        {
            agent.isStopped = false;
            anim.SetFloat("Speed", speed);
            speed += Time.deltaTime * 3;
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", speed);
            speed -= Time.deltaTime * 5;

            Vector3 direction = (fishingPosition.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);


            anim.SetBool("Fishing", true);
        }
        speed = Mathf.Clamp(speed, 0, 1.5f);
    }

}
