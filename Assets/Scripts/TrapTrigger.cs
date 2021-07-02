using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.gameStart)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                GameManager.instance.GameEnd();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.gameStart)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                GameManager.instance.GameEnd();
            }
        }
    }
}
