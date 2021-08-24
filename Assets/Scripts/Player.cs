using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{

    [SerializeField] float jumpVelocity = 10f;
    [SerializeField] float dashVelccity = 2f;
    [SerializeField] float rollingVelocity = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private List<GameObject> creatablePrefabList = new List<GameObject>();
    private Dictionary<string, GameObject> creatablePrefabTree;
    [SerializeField] private Transform shotPos;
    [SerializeField] private BoxCollider2D swordCollider;

    [SerializeField] private Equipment equipment;

    private Rigidbody2D rigidbody2d;

    public Vector3 movePos;
    // Update is called once per frame

    private void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        creatablePrefabTree = creatablePrefabList.ToDictionary(o => o.name);
        // animator = GetComponent<Animator>();
    }

    public void StartMove()
    {
        movePos.x = 5;
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        rigidbody2d.velocity = new Vector2(0, 0);
        rigidbody2d.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        rigidbody2d.velocity = Vector2.right * jumpVelocity;
        animator.SetTrigger("Dash");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Sword_Attack);
    }

    public void Rolling()
    {
        rigidbody2d.velocity = Vector2.right * rollingVelocity;
        animator.SetTrigger("Rolling");
    }

    public void Sit()
    {
        animator.SetTrigger("Sit");
    }

    public void Stop()
    {
        animator.SetTrigger("Die");
        movePos.x = 0;
    }

    public void ArrowShot()
    {
        GameObject arrowObj;
        if (this.shotPos == null)
            return;

        if(creatablePrefabTree.TryGetValue("Arrow",out arrowObj) == true)
        {
            GameObject.Instantiate(arrowObj, this.shotPos.position,Quaternion.identity);
        }
    }

    public bool IsJumping()
    {
        if (this.rigidbody2d.velocity.sqrMagnitude <= 0.001f)
            return false;

        return true;
    }

    public void SwordColliderOn(bool active)
    {
        this.swordCollider.enabled = active;
    }

    public void Equip(string equipName)
    {
        this.equipment.Equip(equipName);
    }

    public void UnEquip(string equipName)
    {
        this.equipment.UnEquip(equipName);
    }

    public void UnEquipOnAnimEnd(string equipName)
    {
        StartCoroutine(UnEquipCoroutine(equipName));
    }

    IEnumerator UnEquipCoroutine(string equipName)
    {
        while (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        UnEquip(equipName);
    }

    void Update()
    {
        this.transform.position += (movePos * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance.isPlay)
        {
            if (collision.transform.tag.Equals("Item"))
            {
                var item = collision.gameObject.GetComponent<CommandItem>();
                GameManager.instance.GetCommandItem(item.itemData, collision.gameObject.transform.position);
                GameObject.Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isPlay)
        {
            if (collision.transform.tag.Equals("Item"))
            {
                var item = collision.gameObject.GetComponent<CommandItem>();
                GameManager.instance.GetCommandItem(item.itemData, collision.gameObject.transform.position);
                GameObject.Destroy(collision.gameObject);
            }
        }
    }
}
