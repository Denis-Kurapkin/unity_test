using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    public List<Transform> wayPoint;
    public int curWayPoint;

    Animator anim;
    public float speed;

    NavMeshAgent agent;
    public Transform player;
    Transform target;

    public Transform head;
    public int visible;
    public int angleView;

    public int damage;



    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        head = anim.GetBoneTransform(HumanBodyBones.Head).transform;
    }

    void Update()
    {

        if (target == null)
        FindTargetRayCast();

        if (target != null)
        {
            Attack();
        }
        else if (target == null)
        {
            anim.SetBool("Attack", false);
            if (wayPoint.Count > 1)
            {
                if (wayPoint.Count > curWayPoint)
                {
                    agent.SetDestination(wayPoint[curWayPoint].position);
                    float distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);

                    if (distance > 2.5f)
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
                else if (wayPoint.Count == curWayPoint)
                {
                    curWayPoint = 0;
                }

            }
            else if (wayPoint.Count == 1)
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
            speed = Mathf.Clamp(speed, 0, 1.5f);
        }
    }

    public void Attack()
    {
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > 1.5f)
        {
            agent.isStopped = false;
            anim.SetFloat("Speed", speed);
            anim.SetBool("Attack", false);
            speed += Time.deltaTime * 3;
        }
        else
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", speed);
            speed -= Time.deltaTime * 5;

            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

            anim.SetBool("Attack", true);


        }
        speed = Mathf.Clamp(speed, 0, 1.5f);
    }




    public void FindTargetRayCast()
    {
        if (target == null)
        {
            float distance = Vector3.Distance(head.position, player.position);
            if (distance <= visible)
            {
                Quaternion look = Quaternion.LookRotation(player.position - head.position);
                float angle = Quaternion.Angle(head.rotation, look);

                if (angle <= angleView)
                {
                    RaycastHit hit;
                    Debug.DrawLine(head.position, player.position + Vector3.up * 1.6f);
                    if (Physics.Linecast(head.position, player.position + Vector3.up * 1.6f, out hit) && hit.transform != head && hit.transform != transform)
                    {
                        if (hit.transform == player)
                        {
                            target = player;
                        }
                        else
                        {
                            target = null;
                        }
                    }

                }
                else
                {
                    target = null;
                }
            }
            else
            {
                target = null;
            }
        }
        else
        {
            RaycastHit hit;
            Debug.DrawLine(head.position, player.position + Vector3.up * 1.6f);
            if (Physics.Linecast(head.position, player.position + Vector3.up * 1.6f, out hit) && hit.transform != head && hit.transform != transform)
            {
                if (hit.transform == player)
                {
                    target = player;
                }
                else
                {
                    target = null;
                }
            }
        }

    }




    void Patrol()
    {
        if (wayPoint.Count > 1)
        {
            if (wayPoint.Count > curWayPoint)
            {
                agent.SetDestination(wayPoint[curWayPoint].position);
                float distance = Vector3.Distance(transform.position, wayPoint[curWayPoint].position);

                if (distance > 2.5f)
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
            else if (wayPoint.Count == curWayPoint)
            {
                curWayPoint = 0;
            }

        }
        else if (wayPoint.Count == 1)
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
    public void Damage()
    {
        target.GetComponent<HeroStats>().TakeAwayHealth(damage);

        if (target.GetComponent<HeroStats>().health <= 0)
        {
            target = null;
        }
    }

}
