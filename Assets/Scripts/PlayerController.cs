using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variable declaration
    private Rigidbody playerRB;
    public GameObject bombPrefab;
    public GameObject landminePrefab;
    public float speed = 10f;
    private float rotationSpeed = 720;
    private Vector3 movement;
    private float horizontalInput;
    private float verticalInput;
    private bool isOnGround = true;
    private float jumpForce = 5f;
    public float attackRange = 5f;
    private float throwForce = 5f;
    private int punchDamage = 10;
    public LayerMask targetMask;
    private bool isWalking;
    private bool isRunning;
    private Animator playerAnim;
    public Transform attackPoint;
    bool oneBombAtATime = false;
    bool oneLandmineAtATime = false;
    public int health = 100;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground") 
        {
            isOnGround = true;
            isWalking = false;
            playerAnim.SetBool("Jump_b", false);

        }
    }
    void Walking() 
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalInput, 0, verticalInput);
        movement.Normalize();
        playerAnim.SetFloat("Speed_f", 0.5f);
        speed = 10f;
        
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        if (movement != Vector3.zero) 
        {
            Quaternion ToRotate = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotate, rotationSpeed * Time.deltaTime);
        }
    }
    void Sprint() 
    {
        //To Sprint/Run
        isWalking = false;
        isRunning = true;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalInput, 0, verticalInput);
        movement.Normalize();
        playerAnim.SetFloat("Speed_f", 1.0f);
        speed = 15f;

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        if (movement != Vector3.zero)
        {
            Quaternion ToRotate = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotate, rotationSpeed * Time.deltaTime);
        }
    }
    void Bomb() 
    {
        //to throw bombs
        if (isOnGround) 
        {
            if (!oneBombAtATime) 
            {
                //Stop and throw bombs
                playerAnim.SetFloat("Speed_f", 0f);
                playerAnim.SetInteger("Animation_int", 10);
                GameObject bomb = Instantiate(bombPrefab, attackPoint.transform.position, bombPrefab.transform.rotation);
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                rb.AddForce(attackPoint.transform.forward + attackPoint.transform.up * throwForce, ForceMode.Impulse);
                oneBombAtATime = true;
            }
        }
    }
    void BombAttack() 
    {
        Bomb();
        BombDelayAction();
    }
    IEnumerator BombDelay()
    {
        yield return new WaitForSeconds(5);
        oneBombAtATime = false;
    }
    IEnumerator LandmineDelay()
    {
        yield return new WaitForSeconds(5);
        oneLandmineAtATime = false;
    }
    void BombDelayAction()
    {
        StartCoroutine(BombDelay());
    }
    void LandmineDelayAction()
    {
        StartCoroutine(LandmineDelay());
    }
    void Landmine() 
    {
        //To throw Landmine
        if (isOnGround)
        {
            if (!oneLandmineAtATime) 
            {
                //Stop and throw landmines
                playerAnim.SetFloat("Speed_f", 0f);
                playerAnim.SetInteger("Animation_int", 10);
                GameObject landmine = Instantiate(landminePrefab, attackPoint.transform.position, landminePrefab.transform.rotation);
                Rigidbody rb = landmine.GetComponent<Rigidbody>();
                rb.AddForce(attackPoint.transform.forward + attackPoint.transform.up * throwForce);
            }
            oneLandmineAtATime = true;
        }
    }
    void LandmineAttack() 
    {
        Landmine();
        LandmineDelayAction();
    }

    IEnumerator PunchDelay()
    {
        yield return new WaitForSeconds(1);
    }
    void PunchDelayAction() 
    {
        StartCoroutine(PunchDelay());
    }
    void Punch() 
    {
        //List of objects detected by ray cast of attackpoint
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, attackRange, targetMask);
        foreach (Collider target in hitTargets)
        {
            //Damage them
            target.GetComponent<Enemy>().TakeDamage(punchDamage);
        }
    }
    void PunchAttack() 
    {
        Punch();
        PunchDelayAction();
    }
    void Jump() 
    {
        //To jump
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnim.SetBool("Jump_b", true);
        isOnGround = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (isOnGround)
            {
                playerAnim.SetFloat("Speed_f", 0f);
                //To walk
                Walking();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Sprint();
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    playerAnim.SetFloat("Speed_f", 0.5f);
                    speed = 10f;
                    isRunning = false;
                    isWalking = true;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    BombAttack();
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    LandmineAttack();
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    PunchAttack();
                }
            }
        }
        else 
        {
            playerAnim.SetBool("Death_b", true);
        }

    }
}
