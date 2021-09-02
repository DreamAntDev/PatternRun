using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private void Awake()
    {
        var breakableObj = this.gameObject.AddComponent<BreakableObject>();
        breakableObj.TrapToBreakableObject(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(DestroyTime());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.isPlay)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                if (GameManager.instance.PlayerCollision(this) == true)
                {
                    var breakableObject = this.GetComponent<BreakableObject>();
                    breakableObject?.Break();
                }
                else
                {
                    GameManager.instance.GameEnd();
                }
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
                if (GameManager.instance.PlayerCollision(this) == true)
                {
                    var breakableObject = this.GetComponent<BreakableObject>();
                    breakableObject?.Break();
                }
                else
                {
                    GameManager.instance.GameEnd();
                }
            }

            if (collision.transform.tag.Equals("Item"))
            {
                Destroy(gameObject);
            }
        }
    }


    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void SetPosition(Vector3 v)
    {
        transform.position = new Vector3(v.x + 15f, -10f, 0f);
    }

}
