using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSimulation : MonoBehaviour
{
    public static TrapSimulation instance;
    protected List<Trap> onGameTraps = new List<Trap>();

    [SerializeField] Trap[] traps;
    [SerializeField] GameObject parent;
    [SerializeField] List<CommandItem> itemPrefabList;

    //private float[] randomY = { 0f, 0f };
    private float totalTrapWeight;
    private Queue<GameObject> orderTrapQueue = new Queue<GameObject>();
    private Camera cam;
    // x = +8f, up = +5f

    private int trapCount = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cam = Camera.main;
    }

    public void OnSimulation()
    {
        for(int i= 0; i < 3; i++)
        {
            onGameTraps.Add(traps[SearchTrap(i + 1)]);
        }
        StartCoroutine(StartSimulation());
    }

    public void SetTrap(int[] ids)
    {
        for(int i = 0; i < ids.Length; i++)
        {
            int idx = SearchTrap(ids[i]);
            if (idx != -1)
            {
                onGameTraps.Add(traps[idx]);
            }
            else
            {
                Debug.LogError("Search Fail Trap");
            }
        }
    }

    public int SearchTrap(int id)
    {
        int findidx = -1;

        for(int i = 0; i < traps.Length; i++)
        {
            if(traps[i].id == id)
            {
                findidx = i;
                break;
            }
        }

        return findidx;
    }

    IEnumerator StartSimulation()
    {
        for (; ;)
        {
            StartCoroutine(CreateTrap());
            if (!GameManager.instance.isPlay)
            {
                break;
            }
            yield return new WaitForSeconds(13f);
        }
    }

    IEnumerator CreateTrap()
    {
        orderTrapQueue.Clear();
        totalTrapWeight = 0;
        Debug.Log(++trapCount);
        while (true)
        {
            if (onGameTraps.Count > 0)
                break;

            yield return new WaitForEndOfFrame();
        }

        CreateItem();

        while (true)
        {
            Trap trap = onGameTraps[Random.Range(0, onGameTraps.Count)];
            if (GameManager.instance.weight - ((4 - orderTrapQueue.Count) * 5f) >= totalTrapWeight + trap.weight)
            {
                GameObject trapobj = GameObject.Instantiate(trap.obj);
                orderTrapQueue.Enqueue(trapobj);
                totalTrapWeight += trap.weight;
                trapobj.SetActive(false);
            }

            if(orderTrapQueue.Count >= 5)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        isMapChecker = true;
        while(orderTrapQueue.Count > 0)
        {
            var trapobj = orderTrapQueue.Dequeue();
            Vector3 position = new Vector3(GameManager.instance.GetPlayer().transform.position.x + 15f, -10f, 0f);
            trapobj.transform.position += position;
            if (parentMap != null)
            {
                trapobj.transform.parent = parentMap;
            }
            else
            {
                trapobj.transform.parent = parent.transform;
            }
            trapobj.SetActive(true);
            yield return new WaitForSeconds(2.5f);
        }
        isMapChecker = false;

    }

    private void CreateItem()
    {
        //1 = 10m
        switch (trapCount)
        {
            case 1:
                orderTrapQueue.Enqueue(GameObject.Instantiate(this.itemPrefabList[0]).gameObject);
                break;

            case 2:
                orderTrapQueue.Enqueue(GameObject.Instantiate(this.itemPrefabList[1]).gameObject);
                break;

            case 3:
                orderTrapQueue.Enqueue(GameObject.Instantiate(this.itemPrefabList[2]).gameObject);
                break;

            case 4:
                orderTrapQueue.Enqueue(GameObject.Instantiate(this.itemPrefabList[3]).gameObject);
                break;

            case 5:
                orderTrapQueue.Enqueue(GameObject.Instantiate(this.itemPrefabList[4]).gameObject);
                break;
        }
       // GameManager.instance.GetMeter();
    }

    RaycastHit hit;
    bool isMapChecker = false;
    Transform parentMap;
    private void FixedUpdate()
    {
        if (isMapChecker)
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * 11f, Color.yellow);
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity))
            {
                if(hit.transform != parentMap)
                    parentMap = hit.transform;
            }
        }
    }
    

    public void ResetTrapPosition()
    {
        TrapTrigger[] traps = parent.GetComponentsInChildren<TrapTrigger>();
        Debug.Log("TransformLength : " + traps.Length);
        for (int i = 0; i < traps.Length; i++)
        {
            if( i == 0)
            {
                traps[i].transform.position += new Vector3(-100f, 0f, 0f);
            }
            else
            {
                traps[i].SetPosition(new Vector3(traps[i - 1].transform.position.x, 0, 0));
            }
        }
    }
}
