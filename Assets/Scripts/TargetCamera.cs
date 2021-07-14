using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetCamera : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    private void LateUpdate()
    {
        this.transform.position = new Vector3(target.position.x + offset.x, offset.y, this.transform.position.z);
    }
}
