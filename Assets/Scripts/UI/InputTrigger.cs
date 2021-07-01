using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InputPoint") == false)
            return;

        MainUI.Instance.inputPad.Input(collision.gameObject);
    }
}
