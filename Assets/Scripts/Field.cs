using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public SpriteRenderer sprite;
    public BoxCollider2D groundCollider;

    private void Awake()
    {
        this.groundCollider.size = new Vector3(sprite.bounds.size.x/this.gameObject.transform.localScale.x, this.groundCollider.size.y);
        float height = (float)sprite.bounds.size.y / (float)this.gameObject.transform.localScale.y;
        this.groundCollider.transform.localPosition = new Vector3(0,  -height/ 2.0f + this.groundCollider.size.y/2.0f);
    }
    //private void OnBecameInvisible()
    //{
    //    FieldManager.Instance.OnInvisibleField(this.gameObject);
    //}
}
