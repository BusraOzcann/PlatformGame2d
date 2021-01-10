using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Conditions
    [Header("Attack")]
    [SerializeField] private bool attacked;
    [SerializeField] private float currentAttackTimer;
    [SerializeField] private float defaultAttackTimer;
    
    [Header("Velocity Horizontal")]
    [SerializeField] private float mySpeedX;
    [SerializeField] private float speed;

    [Header("Velocity Vertical")]
    public bool onGround;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float jumpSpeed;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    private Vector3 defaultLocalScale;

    [Header("GameObjects")]
    [SerializeField] private GameObject arrow;

    [Header("Animations")]
    [SerializeField] private Animator animator;
    #endregion
    void Start()
    {
        defaultLocalScale = transform.localScale;
        onGround = true;
        attacked = false;
    }

    void Update()
    {
        mySpeedX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(mySpeedX * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(mySpeedX));

        #region Player Movement (Jump - Run)
        if (mySpeedX > 0)
        {
            transform.localScale = defaultLocalScale;
        }
        else if (mySpeedX < 0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x * -1, defaultLocalScale.y, defaultLocalScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                animator.SetTrigger("Jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    canDoubleJump = false;
                }
            }
        }
        #endregion

        #region Player ok atması kontrolu
        if (Input.GetMouseButtonDown(0))
        {
            if (attacked == false)
            {
                animator.SetTrigger("Attack");
                attacked = true;
                Invoke("Fire", 0.6f);
            }
           
        } 

        if (attacked == true)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }

        if (currentAttackTimer <= 0)
        {
            attacked = false;
        }
        #endregion
    }

    private void Fire()
    {
        GameObject Arrow = Instantiate(arrow, transform.position, Quaternion.identity);
        if (transform.localScale.x > 0)
        {
            Arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        }
        else
        {
            Vector3 arrowScale = Arrow.transform.localScale;
            Arrow.transform.localScale = new Vector3(arrowScale.x * -1, arrowScale.y, arrowScale.z);
            Arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Die();
        }
        
    }

    void Die()
    {
        animator.SetTrigger("Die");
        //animator.SetFloat("Speed", 0);
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<PlayerController>().enabled = false; // enabled = false; dersen de aynı işlevi görür.
    }


}
    

