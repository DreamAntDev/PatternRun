using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidBody;
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.rigidBody.AddForce((new Vector2(1, 1)).normalized*10, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.right = this.rigidBody.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
            return;

        else
            Destroy(this.gameObject);
    }
}
