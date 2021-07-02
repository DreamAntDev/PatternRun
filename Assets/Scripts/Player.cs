using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float dashVelccity = 2f;
    [SerializeField] float rollingVelocity = 2f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rigidbody2d;

    public Vector3 movePos;
    // Update is called once per frame

    private void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
    }
    public void StartMove()
    {
        movePos.x = 5;
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

    public void Attack()
    {
        animator.SetTrigger("Attack");
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
        animator.SetTrigger("Die");
        movePos.x = 0;
    }

    void Update()
    {
        this.transform.position += (movePos * Time.deltaTime);
    }
}
