using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.isPlay)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                GameManager.instance.GameEnd();
            }

            if (collision.transform.tag.Equals("Item"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isPlay)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                GameManager.instance.GameEnd();
            }

            if (collision.transform.tag.Equals("Item"))
            {
                Destroy(gameObject);
            }
        }
    }
}
