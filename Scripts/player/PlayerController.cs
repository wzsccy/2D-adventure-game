using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Playerinputcontrol inputControl;
    public Vector2 inputDirection;
    private Rigidbody2D rb;  
    private physicsCheck physicsCheck;
    private playAnimation playAnimation;
    public CapsuleCollider2D coll;
    [Header("基本参数")]
    public float speed;
    public float jumoForce;
    public float hurtForce;
    [Header("物理材质")]
    public PhysicsMaterial2D normal; 
    public PhysicsMaterial2D wall;


    
    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        physicsCheck=GetComponent<physicsCheck>();
        playAnimation = GetComponent<playAnimation>();
        inputControl = new Playerinputcontrol();
        coll = GetComponent<CapsuleCollider2D>();
        //跳跃
        inputControl.Gameplay.jump.started +=jump;

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        CheakState();
    }
    private void FixedUpdate()
    {
        if(!isHurt&&!isAttack)
        Move();
    }
    /*private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
    }*/
    public void Move()
    {
        rb.velocity=new Vector2(inputDirection.x*speed*Time.deltaTime,rb.velocity.y);
        
        int faceDir=(int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x< 0)
            faceDir = -1;
        //人物翻转
         transform.localScale = new Vector3(faceDir,1,1);
    } 
    private void jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("jump");
        if(physicsCheck.isGround)
        rb.AddForce(transform.up * jumoForce, ForceMode2D.Impulse);
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playAnimation.PlayerAttack();
        isAttack = true;
       
    }
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity=Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    private void CheakState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal:wall;
    }
}
