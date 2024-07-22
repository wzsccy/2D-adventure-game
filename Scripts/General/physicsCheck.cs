using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    
    [Header("检测参数")]
    public bool manual;
    public Vector2 bottomOffset; 
    public Vector2 leftOffset; 
    public Vector2 rightOffset; 
    public float checkRaduis;
    public LayerMask groundlayer;
    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            rightOffset = new Vector2(rightOffset.x, rightOffset.y);
        }
    }
    void Update()
    {
        Check();
    }
    public void Check()
    {
        //检测是否在地面
        isGround=Physics2D.OverlapCircle(((Vector2)transform.position+bottomOffset)*transform.localScale.x, checkRaduis, groundlayer);
        //检测墙体
         touchLeftWall=Physics2D.OverlapCircle((Vector2)transform.position+leftOffset, checkRaduis, groundlayer);   
         touchRightWall=Physics2D.OverlapCircle((Vector2)transform.position+rightOffset, checkRaduis, groundlayer);   
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
