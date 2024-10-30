//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class physicsCheck : MonoBehaviour
//{
//    private CapsuleCollider2D coll;

//    [Header("������")]
//    public bool manual;
//    public Vector2 bottomOffset;
//    public Vector2 leftOffset;
//    public Vector2 rightOffset;
//    public float checkRaduis;
//    public LayerMask groundlayer;

//    [Header("״̬")]
//    public bool isGround;
//    public bool touchLeftWall;
//    public bool touchRightWall;

//    private void Awake()
//    {
//        coll = GetComponent<CapsuleCollider2D>();
//        UpdateOffsets(transform.localScale.x);
//    }

//    public void UpdateOffsets(float direction)
//    {
//        if (!manual)
//        {

//            leftOffset = new Vector2(-coll.bounds.size.x / 2 * direction, coll.bounds.size.y / 2);
//            rightOffset = new Vector2(coll.bounds.size.x / 2 * direction, coll.bounds.size.y / 2);
//        }
//    }

//    void Update()
//    {
//        Check();
//    }

//    public void Check()
//    {
//        // ������
//        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundlayer);

//        // ���ǽ�ڼ��
//        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundlayer);

//        // �Ҳ�ǽ�ڼ��
//        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundlayer);



//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);

//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);

//        Gizmos.color = Color.green;
//        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb;

    [Header("������")]
    public bool manual;
    public bool isPlayer;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundLayer;

    [Header("״̬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }

        if (isPlayer)
    playerController = GetComponent<PlayerController>();

    }
    private void Update()
{
    Check();
}

public void Check()
{
    //������
    if (onWall)
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
    else
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);

    //ǽ���ж�
    touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
    touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);

    //��ǽ����
    if (isPlayer)
        onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y < 0f;
}

private void OnDrawGizmosSelected()
{
    Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
    Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
    Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
}
}
