using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KwakPlayer : MonoBehaviour
{
    public Vector3 movePos;
    // Update is called once per frame
    void Update()
    {
        this.transform.position += (movePos * Time.deltaTime);
    }
}
