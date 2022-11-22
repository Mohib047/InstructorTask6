using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public GameObject explosionEffect;
    public float force = 700;
    public float radius = 10f;
    private int landmineDamage = 100;

    bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy" && !hasExploded)
        {
            Explosde();
            hasExploded = true;
        }
        else if (collision.collider.tag == "Bomb" && !hasExploded) 
        {
            Explosde();
            hasExploded = true;
        }
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
                    target.GetComponent<Enemy>().TakeDamage(landmineDamage);
                }
                else if (target.gameObject.CompareTag("Player")) 
                {
                    target.gameObject.GetComponent<PlayerController>().TakeDamage(landmineDamage);
                }

            }
        }
        //remove bomb
        Destroy(gameObject);
    }
        

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
