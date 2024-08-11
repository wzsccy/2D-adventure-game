using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public Playerinputcontrol inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private physicsCheck physicsCheck;
    private playAnimation playerAnimation;
    private Character character;

    public Vector2 inputDirection;
    [Header("��������")]
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("�������")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("״̬")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<physicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<playAnimation>();
        character = GetComponent<Character>();

        originalOffset = coll.offset;
        originalSize = coll.size;

        inputControl = new Playerinputcontrol();

        //��Ծ
        inputControl.Gameplay.jump.started += Jump;

        //#region ǿ����·
        //runSpeed = speed;
        //inputControl.Gameplay.WalkButton.performed += ctx =>
        //{
        //    if (physicsCheck.isGround)
        //        speed = walkSpeed;
        //};

        //inputControl.Gameplay.WalkButton.canceled += ctx =>
        //{
        //    if (physicsCheck.isGround)
        //        speed = runSpeed;
        //};
        //#endregion

        //����
        inputControl.Gameplay.Attack.started += PlayerAttack;

        //����
        //inputControl.Gameplay.Slide.started += Slide;
        inputControl.Enable();
    }

    private void OnEnable()
    {
        //inputControl.Enable();
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
       
    }


    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
            Move();
    }

    //����
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     Debug.Log(other.name);
    // }

    //�������ع���ֹͣ����
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }

    //��ȡ��Ϸ����
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    //���ؽ���֮����������
    private void OnAfterSceneLoadedEvent()
    {
        inputControl.Gameplay.Enable();
    }

    public void Move()
    {
        //if (isSlide) return;
        //�����ƶ�
        //if (!isCrouch && !wallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;

        //���﷭ת
        transform.localScale = new Vector3(faceDir, 1, 1);

        //�¶�
        //isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        //if (isCrouch)
        //{
        //    //�޸���ײ���С��λ��
        //    coll.offset = new Vector2(-0.05f, 0.85f);
        //    coll.size = new Vector2(0.7f, 1.7f);
        //}
        //else
        //{
        //    //��ԭ֮ǰ��ײ�����
        //    coll.size = originalSize;
        //    coll.offset = originalOffset;
        //}
    }


    private void Jump(InputAction.CallbackContext obj)
    {
        // Debug.Log("JUMP");
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
   
//        //��ϻ���Э��
//        isSlide = false;
//        StopAllCoroutines();
//    }
//    else if (physicsCheck.onwalk && !wallJump)
//    {
//        rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
//        wallJump = true;
//    }
//}


    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            playerAnimation.PlayerAttack();
            isAttack = true;
        }
    }


    //private void Slide(InputAction.CallbackContext obj)
    //{
    //    if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
    //    {
    //        isSlide = true;

    //        var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

    //        gameObject.layer = LayerMask.NameToLayer("Enemy");
    //        StartCoroutine(TriggerSlide(targetPos));

    //        character.OnSlide(slidePowerCost);
    //    }
    //}

    //private IEnumerator TriggerSlide(Vector3 target)
    //{
    //    do
    //    {
    //        yield return null;
    //        if (!physicsCheck.isGround)
    //            break;

    //        //����������ײǽ
    //        if (physicsCheck.touchLeftWall && transform.localScale.x < 0f || physicsCheck.touchRightWall && transform.localScale.x > 0f)
    //        {
    //            isSlide = false;
    //            break;
    //        }

    //        rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
    //    } while (MathF.Abs(target.x - transform.position.x) > 0.1f);

    //    isSlide = false;
    //    gameObject.layer = LayerMask.NameToLayer("Player");
    //}


    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
       else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);


        //if (wallJump && rb.velocity.y < 0f)
        //{
        //    wallJump = false;
        //}

        //������������˼�������player
        if (isDead)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}

