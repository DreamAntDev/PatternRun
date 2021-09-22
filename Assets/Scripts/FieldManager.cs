using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [System.Serializable]
    public class FieldGenerateData
    {
        public GameObject fieldPrefab;
        public int count;
    }

    private static FieldManager instance;
    FieldGenerateData currentFieldGenerateData;
    int currentFieldGenerateDataIndex = 0;
    Queue<Field> fieldQueue = new Queue<Field>();
    float xSize;
    [SerializeField] Vector3 spawnPos = Vector3.zero; //앞으로 생성할 포지션
    public Player player;
    public List<FieldGenerateData> fieldGenerateDatas = new List<FieldGenerateData>();

    private Field currentField;
    //임시 코드
   /* [SerializeField] List<CommandItem> itemPrefabList;
    private int generatedCount = 0;*/

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
        SetCurrentFieldData(currentFieldGenerateDataIndex);
         /** field.gameObject.transform.localScale.x*/;

       // this.spawnPos.y = -2.5f;
        //this.generatedCount = 0;
        InitField();
    }

    private void SetCurrentFieldData(int idx)
    {
        var index = Mathf.Min(this.fieldGenerateDatas.Count - 1, idx);
        this.currentFieldGenerateData = this.fieldGenerateDatas[index];
        this.currentFieldGenerateDataIndex = index;
        var field = this.currentFieldGenerateData.fieldPrefab.GetComponent<Field>();
        this.xSize = field.sprite.bounds.size.x;
    }

    private void Update()
    {
        if (this.fieldQueue.Count > 0)
        {
            var field = this.fieldQueue.Peek();
            Vector3 fieldRightPos = field.transform.position;
            fieldRightPos.x += (field.sprite.bounds.size.x / 2.0f);
            
            var viewportPoint = Camera.main.WorldToViewportPoint(fieldRightPos);
            if (viewportPoint.x < -1.0f)
            {
                OnInvisibleField(field);
            }
        }
        //if (this.spawnPos.x > 100)
        //{
        //    int queueCount = this.fieldQueue.Count;
        //    for (int i = 0; i < queueCount; i++)
        //    {
        //        var field = this.fieldQueue.Dequeue();
        //        var xPos = field.transform.position.x - this.spawnPos.x;
        //        field.transform.position = new Vector3(xPos, field.transform.position.y, field.transform.position.z);
        //        this.fieldQueue.Enqueue(field);
        //    }
        //    this.player.transform.position = new Vector3(this.player.transform.position.x - this.spawnPos.x, this.player.transform.position.y, this.player.transform.position.z);
            
        //    this.spawnPos.x = 0;
        //}
        
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
        var obj = GameObject.Instantiate(this.currentFieldGenerateData.fieldPrefab, spawnPos, Quaternion.identity, this.transform);
        this.spawnPos.x += xSize;
        var field = obj.GetComponent<Field>();
        this.fieldQueue.Enqueue(field);
        this.currentFieldGenerateData.count--;
        if(this.currentFieldGenerateData.count <= 0)
        {
            this.currentFieldGenerateDataIndex++;
            SetCurrentFieldData(this.currentFieldGenerateDataIndex);
        }
       /* generatedCount++;

        // 임시 코드
        if(generatedCount <= 4)
        {
            var item = GameObject.Instantiate(this.itemPrefabList[generatedCount-1]);
            item.transform.parent = field.transform;
            item.transform.localPosition = new Vector3(2.0f, -1.8f, 0);
        }*/
    }
    private void DestroyField()
    {
        var field = this.fieldQueue.Dequeue();
        GameObject.Destroy(field.gameObject);
    }

    private Field GetCurrentField()
    {
        return currentField;
    }

}
