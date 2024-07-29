
 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyCheck : MonoBehaviour
{

    private CapsuleCollider2D coll;

    [Header("¼ì²â²ÎÊý")]
    public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundlayer;

    [Header("×´Ì¬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    void Update()
    {
        Check();
    }

    public void Check()
    {
        // ¼ì²âÊÇ·ñÔÚµØÃæ
        isGround = Physics2D.OverlapCircle(((Vector2)transform.position + bottomOffset) * transform.localScale.x, checkRaduis, groundlayer);

        // ¼ì²â×ó²àÇ½±Ú
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundlayer);

        // ¼ì²âÓÒ²àÇ½±Ú
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}


