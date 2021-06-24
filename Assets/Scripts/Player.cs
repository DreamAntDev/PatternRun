using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float dashVelccity = 2f;
    [SerializeField] float rollingVelocity = 2f;
    [SerializeField] private Animator animator;
    private GameObject player;
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        rigidbody2d.velocity = new Vector2(0, 0);
        rigidbody2d.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        rigidbody2d.velocity = Vector2.right * jumpVelocity;
        animator.SetTrigger("Dash");
    } 

    public void Rolling()
    {
        rigidbody2d.velocity = Vector2.right * rollingVelocity;
        animator.SetTrigger("Rolling");
    }

    public void Sit()
    {
        animator.SetTrigger("Sit");
    }

    public void Stop()
    {
        animator.SetBool("End", true);
    }
}
