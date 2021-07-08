using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BreakableObject : MonoBehaviour
{
    public int breakCount = 3;
    //private void OnEnable()
    //{
    //    Break();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Break();
    }
    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            Break();
    }
    public void Break()
    {
        GameObject[] tempObjectArray = { this.gameObject };

        for(int i=0;i<this.breakCount;i++)
        {
            GameObject[] array = null;
            if (tempObjectArray == null)
                break;

            foreach(var obj in tempObjectArray)
            {
                Vector3 center = obj.GetComponent<MeshRenderer>().bounds.center;
                //Vector3 center = obj.GetComponent<BoxCollider2D>().bounds.center;
                Vector3 slicePlaneVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                var slicedObjectArray = EzySlice.SlicerExtensions.SliceInstantiate(obj, center, slicePlaneVector);
                if (slicedObjectArray == null)
                    continue;

                Destroy(obj);
                if(array == null)
                {
                    array = slicedObjectArray;
                }
                else
                {
                    array = array.Concat(slicedObjectArray).ToArray();
                }
            }
            tempObjectArray = array;
        }
        foreach(var obj in tempObjectArray)
        {
            var meshCollider = obj.AddComponent<MeshCollider>();
            var rigidBody = obj.AddComponent<Rigidbody>();
            meshCollider.convex = true;

            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            rigidBody.useGravity = true;
            rigidBody.AddForceAtPosition(new Vector3(2, 0, 0), new Vector3(-1, 0, 0),ForceMode.VelocityChange);

            obj.transform.position = this.transform.position;
        }
    }
}
