using UnityEngine;

public class CubeCreate : MonoBehaviour
{
    public float width = 1;
    public float height = 1;
    public Material refMaterial;
    public void Start()
    {
        if(refMaterial == null)
        {
            Destroy(this);
            return;
        }
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = refMaterial;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0),
            new Vector3(0, 0, 1),
            new Vector3(width, 0, 1),
            new Vector3(0, height, 1),
            new Vector3(width, height, 1)
        };
        mesh.vertices = vertices;

        int[] tris = new int[36]
        {
            //fornt
            0, 2, 1,
            2, 3, 1,

            //right
            1,3,5,
            3,7,5,

            //top
            2,6,3,
            3,6,7,

            //back
            5,7,6,
            5,6,4,

            //left
            6,2,4,
            4,2,0,

            //down
            0,1,4,
            1,5,4,
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

        Vector2[] uv = new Vector2[8]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;

        var col = this.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(width, height, 1);
        col.isTrigger = true;
    }
}