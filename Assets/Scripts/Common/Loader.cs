using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public List<GameObject> loadObjectList = new List<GameObject>();

    private void Awake()
    {
        foreach(var obj in loadObjectList)
        {
            var newObj = Instantiate(obj);
        }
    }
}
