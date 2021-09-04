using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class PlayerAnimAdapter : MonoBehaviour
{
    Animator animator;
    Player player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
        if(player == null)
        {
            Debug.LogError("Player�� �θ� ������Ʈ�� �����ϴ�.");
        }
    }
    
    // call by anim
    private void OnAttackEvent(int param)
    {
        this.player.SwordColliderOn(param == 1);
    }
}
