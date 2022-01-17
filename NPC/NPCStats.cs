using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    Animator anim;

    public float health;
    public GameObject enemy;

    public Transform[] element;
    public Transform item;
    CapsuleCollider capsuleCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void TakeAwayHealth(int takeAway)
    {
        health -= takeAway;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        anim.enabled = false;
        capsuleCollider.enabled = false;

        foreach (Transform body in element)
            body.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<EnemyController>().enabled = false;

        Destroy(enemy, 10f);
       // obj.QEvent.end_Quest1 = true;

    }

 
}
