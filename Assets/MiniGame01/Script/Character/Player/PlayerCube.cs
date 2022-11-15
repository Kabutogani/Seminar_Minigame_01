using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCube : MonoBehaviour
{

    //InputSystemのやつ
    private InputAction moveInput, fireInput, barrierInput;

    //入力を受ける
    private Vector2 move; 

    //プレイヤーのの移動速度
    public float playerSpeed = 0;
    //プレイヤーのデフォルト移動速度
    public float defaultPlayerSpeed = 0;


    private Rigidbody rb;

    public GameObject mouseCursorPoint;

    public GameObject ShootPoint;

    //最大HP
    public int MaxHP;
    //現在HP
    public int CurrentHP;

    //射撃クールタイム
    private float shootCoolTime;
    //チャージ時間
    public float chargeTime;
    //↑の最大値
    public float maxShootCoolTime = 10;
    //弾
    public GameObject Bullet1,Bullet2,Bullet3;
    //チャージする弾(見かけだけ)
    public GameObject charge1, charge2, charge3;
    //今どこまでチャージしてるか
    public int curentChargePower;
    //前フレームに押してたかどうか
    private float lastframeCharge;

    //ガード中か否か
    public bool isGuard;
    //Barrier―オブジェクト
    public GameObject barrierObj;

    //プレイヤーのアニメーター
    private Animator animator;
    //リボルマン本体
    public GameObject revolman;


    // Start is called before the first frame update
    void Start()
    {
        //inputSystem用のコンポーネントをGet
        var playerInput = GetComponent<PlayerInput>();
        //InputSystemの中の現在のアクションマップを取得
        var actionMap = playerInput.currentActionMap;

        //プレイヤーのリジッドボディ
        rb = GetComponent<Rigidbody>();

        moveInput = actionMap["Move"];
        fireInput = actionMap["Fire"];
        barrierInput = actionMap["Barrier"];

        CurrentHP = MaxHP;
        isGuard = false;
        playerSpeed = defaultPlayerSpeed;

        //アニメーター
        animator = revolman.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdata();

    }

    void FixedUpdate(){

        AngleChange();
        Move();
        if(shootCoolTime >= 0f){
            shootCoolTime = shootCoolTime - Time.deltaTime;
        }

    }

    void InputUpdata(){

        //アクションマップに設定されたアクションからMove入力を取得
        move = moveInput.ReadValue<Vector2>();
        
        //Fireボタンが押れたのかの確認
        if(fireInput.triggered){
            Shoot();
        }
        //Fireボタンが押しっぱなしかどうかの確認
        if(fireInput.IsPressed()){
            chargeTime += Time.deltaTime;
            Charge();
        }else{
            charge1.SetActive(false);
            charge2.SetActive(false);
            charge3.SetActive(false);

            animator.SetBool("IsCharge", false);
            animator.SetBool("IsChargeMax", false);
        }
        if(barrierInput.IsPressed()){
            isGuard = true;
            barrierObj.SetActive(true);
            playerSpeed = defaultPlayerSpeed / 8;
        }else{
            isGuard = false;
            barrierObj.SetActive(false);
            playerSpeed = defaultPlayerSpeed;
        }
        
        lastframeCharge = chargeTime;
    }

    void Move(){
        Vector3 movementVelocity = move.normalized * playerSpeed;
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.y);
    }

    void AngleChange(){
        Vector3 lookfor = new Vector3(mouseCursorPoint.transform.position.x, transform.position.y, mouseCursorPoint.transform.position.z);
        transform.LookAt(lookfor);
    }

    void Shoot(){
        GameObject bullet = Bullet1;
        if(shootCoolTime <= 0 && !isGuard){
            switch(curentChargePower){
                case 0:
                    bullet = Instantiate(Bullet1);
                break;
                case 1:
                    bullet = Instantiate(Bullet2);
                break;
                case 2:
                    bullet = Instantiate(Bullet3);
                break;
            }

            bullet.transform.position = ShootPoint.transform.position;
            bullet.transform.rotation = transform.rotation;
            shootCoolTime = maxShootCoolTime;
            curentChargePower = 0;
            chargeTime = 0f;

            animator.SetTrigger("IsFire");
        }
    }
    
    void Charge(){
        if(chargeTime >= 0 && chargeTime <= 0.5f){
            charge1.SetActive(true);
            curentChargePower = 0;
            animator.SetBool("IsCharge", true);
        }
        if(chargeTime >= 0.5 && chargeTime <= 1.5f){
            charge2.SetActive(true);
            curentChargePower = 1;
            animator.SetBool("IsCharge", true);
        }
        if(chargeTime >= 1.5f){
            charge3.SetActive(true);
            curentChargePower = 2;
            animator.SetBool("IsChargeMax", true);
        }
        
    }

    public void Death(){
        Destroy(gameObject);
    }
}
