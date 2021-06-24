using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    private static FieldManager instance;
    public GameObject fieldPrefab;
    Queue<Field> fieldQueue = new Queue<Field>();
    float xSize;
    Vector3 spawnPos = Vector3.zero; //앞으로 생성할 포지션
    public KwakPlayer player;
    public static FieldManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        this.xSize = this.fieldPrefab.GetComponent<Field>().sprite.size.x;
        InitField();
    }
    private void Update()
    {
        if (this.fieldQueue.Count > 0)
        {
            var field = this.fieldQueue.Peek();
            Vector3 fieldRightPos = field.transform.position;
            fieldRightPos.x += (field.sprite.size.x / 2.0f);
            var viewportPoint = Camera.main.WorldToViewportPoint(fieldRightPos);
            if (viewportPoint.x < 0.0f)
            {
                OnInvisibleField(field);
            }
        }
        if(this.spawnPos.x > 100)
        {
            int queueCount = this.fieldQueue.Count;
            for(int i=0;i<queueCount;i++)
            {
                var field = this.fieldQueue.Dequeue();
                field.transform.position -= this.spawnPos;
                this.fieldQueue.Enqueue(field);
            }
            this.player.transform.position -= this.spawnPos;
            this.spawnPos = Vector3.zero;
        }
    }
    public void InitField()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateField();
        }
    }

    public void OnInvisibleField(Field field)
    {
        if (this.fieldQueue.Peek().Equals(field) == false)
        {
            return;
        }
        DestroyField();
        CreateField();
    }
    private void CreateField()
    {
        var obj = GameObject.Instantiate(this.fieldPrefab, spawnPos, Quaternion.identity, this.transform);
        this.spawnPos.x += xSize;
        this.fieldQueue.Enqueue(obj.GetComponent<Field>());
    }
    private void DestroyField()
    {
        var field = this.fieldQueue.Dequeue();
        GameObject.Destroy(field.gameObject);
    }
}
