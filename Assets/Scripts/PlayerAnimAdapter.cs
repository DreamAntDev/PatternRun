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
            Debug.LogError("Player가 부모 오브젝트에 없습니다.");
        }
        SetAttackEvent();
    }

    private void SetAttackEvent()
    {
        if(this.player != null)
        {
            var clip = this.animator.runtimeAnimatorController.animationClips.First(o => o.name.Equals("Attack"));
            AnimationEvent beginEvt = new AnimationEvent();
            beginEvt.time = 0.23f;
            beginEvt.functionName = "OnAttackEvent";
            beginEvt.intParameter = 1;

            AnimationEvent endEvt = new AnimationEvent();
            endEvt.time = clip.length;
            endEvt.functionName = "OnAttackEvent";
            endEvt.intParameter = 0;

            clip.AddEvent(beginEvt);
            clip.AddEvent(endEvt);
        }
    }
    
    // call by anim
    private void OnAttackEvent(int param)
    {
        this.player.SwordColliderOn(param == 1);
    }
}
