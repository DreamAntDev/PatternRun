using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject lockObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        lockObject.SetActive(false);
        this.GetComponent<Collider2D>().enabled = false;
    }
}
