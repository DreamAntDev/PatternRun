using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BreakableObjectCreater))]
public class BreakableObjectCreaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BreakableObjectCreater script = (BreakableObjectCreater)target;
        if(GUILayout.Button("오브젝트 생성"))
        {
            this.CreateBreakableObject(script);
        }
    }

    public void CreateBreakableObject(BreakableObjectCreater script)
    {
        if (script.refMaterial == null)
        {
            Debug.LogError("Material이 없습니다.");
            //Destroy(this);
            return;
        }
        MeshRenderer meshRenderer = script.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = script.refMaterial;
        float width = script.refMaterial.mainTexture.width / 100.0f;
        float height = script.refMaterial.mainTexture.width / 100.0f;

        MeshFilter meshFilter = script.gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0),
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

        var col = script.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(width, height, 1);
        col.isTrigger = true;

        script.gameObject.AddComponent<BreakableObject>();

        var removeScript = script.gameObject.GetComponent<BreakableObjectCreater>();
        DestroyImmediate(removeScript);
    }
}
