using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSimulation : MonoBehaviour
{
    protected List<Trap> onGameTraps = new List<Trap>();

    [SerializeField] Trap[] traps;
    [SerializeField] GameObject parent;

    //private float[] randomY = { 0f, 0f };
    private float totalTrapWeight;
    private Queue<GameObject> orderTrapQueue = new Queue<GameObject>();
    // x = +8f, up = +5f

    public void OnSimulation()
    {
        StartCoroutine(StartSimulation());
       // randomY[0] = GameManager.instance.GetPlayer().transform.position.y;
    }

    public void SetTrap(int[] ids)
    {
        for(int i = 0; i < ids.Length; i++)
        {
            int idx = SearchTrap(ids[i]);
            if (idx != -1)
            {
                onGameTraps.Add(traps[SearchTrap(ids[i])]);
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
        for(; ;)
        {
            StartCoroutine(CreateTrap());
            if (!GameManager.instance.isPlay)
            {
                break;
            }
            yield return new WaitForSeconds(15f);
        }
    }

    IEnumerator CreateTrap()
    {
        orderTrapQueue.Clear();
        while(true)
        {
            if (onGameTraps.Count > 0)
                break;

            yield return new WaitForEndOfFrame();
        }
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

        while(orderTrapQueue.Count > 0)
        {
            var trapobj = orderTrapQueue.Dequeue();
            trapobj.transform.position += new Vector3(GameManager.instance.GetPlayer().transform.position.x + 15f, -10f, 0f);
            trapobj.transform.parent = parent.transform;
            trapobj.SetActive(true);
            yield return new WaitForSeconds(2.5f);
        }

        totalTrapWeight = 0;
    }
}
