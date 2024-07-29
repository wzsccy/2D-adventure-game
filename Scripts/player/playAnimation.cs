using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private physicsCheck physicsCheck;
    private PlayerController playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<physicsCheck>();
        playerController=GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround",physicsCheck.isGround);
        anim.SetBool("isDead",playerController.isDead);
        anim.SetBool("isAttack",playerController.isAttack);
      
    }
    public void PlayerHurt()
    {
        anim.SetTrigger("Hurt");
    }
    public void PlayerAttack() {
        anim.SetTrigger("attack");
    }
}
