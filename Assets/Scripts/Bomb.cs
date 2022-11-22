using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //Variable Declaration
    public GameObject explosionEffect;
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700;
    private int bombDamage = 100;

    float countdown;
    bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bomb" || collision.collider.tag == "Landmine") 
        {
            Explosde();
            hasExploded = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }
    void Explosde() 
    {
        
        //Exploding bomb
        //Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //Get nerby enemies
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider target in colliders) 
        {
            //add force
            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
                if (target.gameObject.CompareTag("Enemy")) 
                {
                    target.GetComponent<Enemy>().TakeDamage(bombDamage);
                }
                else if (target.gameObject.CompareTag("Player"))
                {
                    target.gameObject.GetComponent<PlayerController>().TakeDamage(bombDamage);
                }

            }
        }
        //remove bomb
        Destroy(gameObject);
    }
   
    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded) 
        {
            Explosde();
            hasExploded = true;
        }
    }
}
