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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitSensor")
            Break();
    }

    public void Break()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Crash_Trap);

        GameObject[] tempObjectArray = { this.gameObject };
        var tempMeshRender = this.gameObject.GetComponent<MeshRenderer>();
        var sortingLayerID = tempMeshRender.sortingLayerID;
        var layer = this.gameObject.layer;

        for(int i=0;i<this.breakCount;i++)
        {
            GameObject[] array = null;
            if (tempObjectArray == null)
                break;

            foreach(var obj in tempObjectArray)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                Vector3 center = meshRenderer.bounds.center;
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
            var meshRenderer = obj.GetComponent<MeshRenderer>();

            obj.layer = layer;
            meshRenderer.sortingLayerID = sortingLayerID;

            meshCollider.convex = true;

            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            rigidBody.useGravity = true;
            rigidBody.AddForceAtPosition(new Vector3(2, 0, 0), new Vector3(-1, 0, 0),ForceMode.VelocityChange);

            obj.transform.position = this.transform.position;
            obj.transform.parent = this.transform.parent;
        }
    }

    public void TrapToBreakableObject(GameObject trap)
    {
        var sprite = trap.GetComponent<SpriteRenderer>();
        //if (script.refMaterial == null)
        //{
        //    Debug.LogError("Material이 없습니다.");
        //    //Destroy(this);
        //    return;
        //}
        var tempTexture = sprite.sprite.texture;
        var tempMaterial = new Material(sprite.sharedMaterial);
        tempMaterial.mainTexture = tempTexture;
        var tempSortingLayerID = sprite.sortingLayerID;
        DestroyImmediate(sprite);

        MeshRenderer meshRenderer = trap.AddComponent<MeshRenderer>();
        if(tempMaterial != null || meshRenderer.sharedMaterial != null)
            meshRenderer.sharedMaterial = tempMaterial;
        meshRenderer.sortingLayerID = tempSortingLayerID;

        //float width = tempTexture.width / 100.0f;
        //float height = tempTexture.height / 100.0f;

        float xSize = (tempTexture.width / 100.0f) / 2.0f;
        float ySize = (tempTexture.height / 100.0f) / 2.0f;

        MeshFilter meshFilter = trap.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-xSize,-ySize,0),
            new Vector3(xSize,-ySize,0),
            new Vector3(-xSize,ySize,0),
            new Vector3(xSize,ySize,0),
            //new Vector3(0, 0, 0),
            //new Vector3(width, 0, 0),
            //new Vector3(0, height, 0),
            //new Vector3(width, height, 0),
            //new Vector3(0, 0, 1),
            //new Vector3(width, 0, 1),
            //new Vector3(0, height, 1),
            //new Vector3(width, height, 1)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            //fornt
            0, 2, 1,
            2, 3, 1,

            ////right
            //1,3,5,
            //3,7,5,

            ////top
            //2,6,3,
            //3,6,7,

            ////back
            //5,7,6,
            //5,6,4,

            ////left
            //6,2,4,
            //4,2,0,

            ////down
            //0,1,4,
            //1,5,4,
        };
        mesh.triangles = tris;

        mesh.RecalculateNormals();
        //Vector3[] normals = new Vector3[4]
        //{
        //    -Vector3.forward,
        //    -Vector3.forward,
        //    -Vector3.forward,
        //    -Vector3.forward
        //};
        //mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),

            //new Vector2(0, 0),
            //new Vector2(0, 0),
            //new Vector2(0, 0),
            //new Vector2(0, 0),
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;

        var col = trap.GetComponent<BoxCollider2D>();
        //col.size = new Vector3(width, height, 1);
        col.size = new Vector3(xSize * 2, ySize * 2);
        col.isTrigger = false;

        //trap.AddComponent<BreakableObject>();

        //var removeScript = script.gameObject.GetComponent<BreakableObjectCreater>();
        //DestroyImmediate(removeScript);
    }
}
