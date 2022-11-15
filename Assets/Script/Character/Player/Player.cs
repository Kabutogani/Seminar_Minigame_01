using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    //移動を入力しているかどうか
    private bool isInputUp;
    private bool isInputDown;
    private bool isInputRight;
    private bool isInputLeft;

    //
    private Rigidbody PlayerRigidbody;

    //プレイヤーの移動スピード
    public float PlayerMoveSpeed;
    //プレイヤーのスピード基準値
    private float PlayerNormalMoveSpeed = 0;

    //プレイヤーの攻撃力
    public int PlayerAttackPower;

    //前フレームの座標
    private Vector3 lastFramePosition;

    //攻撃モーションを取れるかどうか
    private bool canAttack;
    public bool isAttack;

    private Animator animator;

    //通常攻撃のヒットボックスのPrefab
    public GameObject PlayerAttackBoxPrefab;
    //通常攻撃のヒットボックスのprefabのスクリプト
    public static PlayerAttackHitBox playerAttackHitBox;
    //通常攻撃のヒットボックスを出現させる位置に置いたゲームオブジェクト
    public GameObject HitBoxCreatePositionObject;

    //魔法攻撃の球
    public GameObject magicBall;
    //魔法攻撃のスクリプト
    public static PlayerMagicAttack playerMagicAttack;

    //Cursorのオブジェクト
    public GameObject MouseCursor;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        PlayerNormalMoveSpeed = PlayerMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        AngleChange();
        Attack();
        MagicAttack();
        CheckCanAttack();
        Debug.Log(isAttack);
    }

    //移動
    private void Move() {

        if(Input.GetAxisRaw("Horizontal") != 0) {
            //InputManagerで設定したキーの入力量を代入
            float movePowerX = Input.GetAxisRaw("Horizontal");
            float movePowerZ = Input.GetAxisRaw("Vertical");
        
            //重力要改善
            Vector3 movementPower = new Vector3(movePowerX, PlayerRigidbody.velocity.y, movePowerZ);
            
            Vector3 movementVelocity = movementPower.normalized *PlayerMoveSpeed;

            //velocityバージョン
            PlayerRigidbody.velocity = new Vector3(movementVelocity.x, PlayerRigidbody.velocity.y, movementVelocity.z);
            animator.SetBool("IsMove", true);
            
        }else {
            animator.SetBool("IsMove", false);
        }

    }

    //移動方向に向きを変える
    private void AngleChange() {
        
        Vector3 moveAngle = transform.position - lastFramePosition;
        //最終フレームのpositionの更新
        lastFramePosition = transform.position; 

        //移動していて、なおかつ攻撃中じゃないなら
        
        if(moveAngle.magnitude > 0.01f && isAttack == false && canAttack) {
            Vector3 angle = new Vector3(moveAngle.x,0, moveAngle.z).normalized;
            transform.rotation = Quaternion.LookRotation(angle);
        }
    }


    //攻撃モーションを取れるかどうかを確認
    private void CheckCanAttack() {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run")){
            if(isAttack == false){
                canAttack = true;
            } else {
                canAttack = false;
            }

        }else{

            canAttack = false;

        }
    }

    private void Attack() {
        if(isAttack) {
            
            //攻撃中は移動速度が遅くなる
            PlayerMoveSpeed = PlayerNormalMoveSpeed / 4;
        }

        if(canAttack == true && Input.GetButtonDown("Fire1")){

        //プレイヤーマウスカーソルの方を向く
        Vector3 corsorVector = MouseCursor.transform.position - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(corsorVector);
        transform.rotation = quaternion;
            animator.SetTrigger("IsAttack");
            isAttack = true;

        }
    }

    void AttackHitBoxCreate() {
        GameObject boxPrefab = Instantiate(PlayerAttackBoxPrefab);
        boxPrefab.transform.position = HitBoxCreatePositionObject.transform.position;
        playerAttackHitBox = boxPrefab.GetComponent<PlayerAttackHitBox>();
        //ヒットボックスの攻撃力を設定
        playerAttackHitBox.AttackPower = PlayerAttackPower;

    }

    void AttackEnd(){
        isAttack = false;
        PlayerMoveSpeed = PlayerNormalMoveSpeed;
    }

    private void MagicAttack() {
        if(isAttack) {
            
            //攻撃中は移動速度が遅くなる
            PlayerMoveSpeed = 0;
        }

        if(canAttack == true && Input.GetButtonDown("Fire2")){

        //プレイヤーマウスカーソルの方を向く
        Vector3 corsorVector = MouseCursor.transform.position - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(corsorVector);
        transform.rotation = quaternion;
            animator.SetTrigger("IsMagic");
            isAttack = true;

        }
    }

    void MagicAttackCreate() {
        GameObject magicPrefab = Instantiate(magicBall);
        magicPrefab.transform.position = HitBoxCreatePositionObject.transform.position;
        playerMagicAttack = magicPrefab.GetComponent<PlayerMagicAttack>();
        //ヒットボックスの攻撃力を設定
        playerMagicAttack.AttackPower = PlayerAttackPower;
        playerMagicAttack.transform.rotation = transform.rotation;

    }

    void MagicEnd(){
        isAttack = false;
        PlayerMoveSpeed = PlayerNormalMoveSpeed;
    }
}
