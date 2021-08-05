using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Trap")]
public class Trap : ScriptableObject
{
    public enum TrapType
    {
        INSTALL,
        GIFTBOX
    }

    public int id;
    public float weight;
    public TrapType type;
    public GameObject obj;
}

