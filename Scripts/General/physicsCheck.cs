using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCheck : MonoBehaviour
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
        UpdateOffsets(transform.localScale.x);
    }

    public void UpdateOffsets(float direction)
    {
        if (!manual)
        {
            
            leftOffset = new Vector2(-coll.bounds.size.x / 2 * direction, coll.bounds.size.y/2);
            rightOffset = new Vector2(coll.bounds.size.x / 2 * direction, coll.bounds.size.y / 2);
        }
    }

    void Update()
    {
        Check();
    }

    public void Check()
    {
        // µØÃæ¼ì²â
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundlayer);

        // ×ó²àÇ½±Ú¼ì²â
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundlayer);

        // ÓÒ²àÇ½±Ú¼ì²â
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundlayer);

        // µ÷ÊÔÊä³ö
        Debug.Log("isGround: " + isGround);
        Debug.Log("touchLeftWall: " + touchLeftWall);
        Debug.Log("touchRightWall: " + touchRightWall);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
