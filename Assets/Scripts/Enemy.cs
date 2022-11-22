using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    //Variable declaration
    private Animator enemyAnim;
    public NavMeshAgent agent;
    public GameObject bombPrefab;
    private float throwForce = 5f;
    private GameObject player;
    private bool playerInRange;
    private float radius = 5f;
    public Transform attackPoint;
    bool oneBombAtATime = false;
    public int health = 100;

    void Bomb()
    {
        //to throw bombs
        GameObject bomb = Instantiate(bombPrefab, attackPoint.transform.position, bombPrefab.transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.AddForce(attackPoint.transform.forward + attackPoint.transform.up * throwForce, ForceMode.Impulse);
        oneBombAtATime = true;
    }

    void Attacking() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider player in colliders)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                playerInRange = true;
            }
        }
        if (playerInRange)
        {
            if (!oneBombAtATime)
            {
                Bomb();
                DelayAction();
            }

        }
    }

    void DelayAction() 
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);
        oneBombAtATime = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            enemyAnim.SetFloat("Speed_f", 0.5f);
            agent.SetDestination(player.transform.position);
            playerInRange = false;
            Attacking();
        }
        else 
        {
            enemyAnim.SetBool("Death_b", true);
            Destroy(gameObject, 1f);
        }
    }
}
