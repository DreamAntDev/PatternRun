using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSimulation : MonoBehaviour
{
    [SerializeField] protected GameObject[] traps;

    [SerializeField] protected int maxTrapCount = 3;
    [SerializeField] protected float topItemY = 0f;

    private float[] randomY = { 0f, 0f };

    // x = +8f, up = +5f

    public void OnSimulation()
    {
        StartCoroutine(StartSimulation());
        randomY[0] = GameManager.instance.GetPlayer().transform.position.y;
    }

    public void GetItem(string icon)
    {
        switch (icon)
        {
            case "sit":
                maxTrapCount++;
                break;

            case "attack":
                maxTrapCount++;
                break;
        }
    }

    IEnumerator StartSimulation()
    {
        for(; ;)
        {
            CreateTrap();
            if (!GameManager.instance.isPlay)
            {
                break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void CreateTrap()
    {
        GameObject trap = GameObject.Instantiate(traps[Random.Range(0, maxTrapCount)]);
        trap.transform.position += new Vector3(GameManager.instance.GetPlayer().transform.position.x + 15f, -10f, 0f);
    }
}
