using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //最大HP
    public int maxHP;
    //現在HP
    public int currentHP;

    //AI用の時間経過測定
    private float elapsedTime;

    //プレイヤーのオブジェクト
    public GameObject playerObj;

    //弾幕用の弾
    public GameObject Bullet1, Bullet2, Bullet3;

    //現在の行動
    public enum State{Idle, generalShot, trackShot, scatterShot, bombShot, hipDrop, hipDrop_Down};
    public State CurrentState{ get; private set; }

    //一個前の行動
    public State LastState{get; private set; }


    //ヒップドロップが上がるまでの時間
    public float soarWaitTime;

    //Idle中に移動する座標
    public Vector3 moveTarget;
    //移動にかける時間
    public float moveSpeed;


    private Rigidbody rb;

    //現在のステートを持続する時間
    private float IdleTime;

    //Shotの射撃間隔
    private float shotCoolTime;
    //Shotの射撃間隔(最大)
    private float maxShotCoolTIme;


    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        ChangeState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate(){
        IdleTime -= Time.deltaTime;
        UpdateState();
        shotCoolTime -= Time.deltaTime;
    }


    
    private void ChangeState(State state) {
        bool isValid = true;

        switch(state) {
            case State.Idle:
                IdleTime = 2f;
                moveTarget = new Vector3(Random.Range(-8, 8), transform.position.y, Random.Range(-8, 8));
                transform.LookAt(moveTarget);
                break;

            case State.generalShot:
                IdleTime = 10f;
                maxShotCoolTIme = 0.5f;
                break;
            case State.trackShot:
                IdleTime = 10f;
                maxShotCoolTIme = 3f;
                break;

            case State.scatterShot:
                IdleTime = 6f;
                maxShotCoolTIme = 1f;
                break;
            case State.bombShot:
                IdleTime = 6f;
                maxShotCoolTIme = 1.5f;
                BombShot();
                break;
            case State.hipDrop:
                IdleTime = 30f;
                rb.useGravity= false;
                soarWaitTime = 2f;
                break;
            case State.hipDrop_Down:
                rb.useGravity = true;
                transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z);
                break;
            default:
                isValid = false;
                break;
        }

        if(!isValid) {
            return;
        }

        CurrentState = state;
        Debug.Log(CurrentState);
    }

    private void UpdateState() {
        bool isValid = true;

        switch(CurrentState) {
            case State.Idle:
                IdleMove();
                break;

            case State.generalShot:
                GeneralShot();
                break;
            case State.trackShot:
                TrackShot();
                break;

            case State.scatterShot:
                ScatterShot(Bullet1);
                break;
            case State.bombShot:
                BombShot();
                break;
            case State.hipDrop:

                HipDrop(playerObj.transform.position);
                break;
            case State.hipDrop_Down:
                HipDropDown();
                break;
            default:
                isValid = false;
                break;
        }

        if(IdleTime <= 0){
            if(CurrentState != State.Idle){
                ChangeState(State.Idle);
            }else{
                ChangeStateRandom();
            }
        }

        if(!isValid) {
            return;
        }

    }


    void NormalShot(Vector3 target, GameObject bulletType){

        GameObject bullet = Instantiate(bulletType);

        bullet.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Bullet>().playerObj = playerObj;
        if(bulletType == Bullet3){
            bullet.GetComponent<EBullet05>().playerObj = playerObj;
        }
        Vector3 shootFor = new Vector3(target.x, 1f, target.z);
        bullet.transform.LookAt(shootFor);
    }

    void SetAngleShotForTarget(Vector3 target, GameObject bulletType,float changeAngle){

        GameObject bullet = Instantiate(bulletType);

        bullet.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 shootFor = new Vector3(target.x, 1, target.z);
        if(bulletType == Bullet3){
            bullet.GetComponent<EBullet05>().playerObj = playerObj;
        }
        bullet.transform.LookAt(shootFor);
        bullet.transform.Rotate(0, changeAngle, 0);
    }


    public void Death(){
        Destroy(gameObject);
    }

    void GeneralShot(){

        if(shotCoolTime <= 0){
            NormalShot(playerObj.transform.position, Bullet1);
            shotCoolTime = maxShotCoolTIme;
        }
    }

    void TrackShot(){

        if(shotCoolTime <= 0){
            NormalShot(playerObj.transform.position, Bullet3);
            shotCoolTime = maxShotCoolTIme;
        }
    }

    void BombShot(){

        if(shotCoolTime <= 0){
            NormalShot(playerObj.transform.position, Bullet2);
            shotCoolTime = maxShotCoolTIme;
        }
    }

    void ScatterShot(GameObject bulletType){
        if(shotCoolTime <= 0){
            for(int shotCount = -10; shotCount <= 10;){
                for(int shotCount2 = -1; shotCount2 <= 2; ++shotCount2){
                    SetAngleShotForTarget(playerObj.transform.position, bulletType, shotCount2*90f + shotCount);
                }
                shotCount = shotCount + 10;
            }
            shotCoolTime = maxShotCoolTIme;
        }
    }

    void HipDrop(Vector3 target){
        if(soarWaitTime >= 0){
            transform.position = new Vector3(transform.position.x, transform.position.y+(30f*Time.deltaTime), transform.position.z);
            soarWaitTime -=Time.deltaTime;
        }else{
            ChangeState(State.hipDrop_Down);
        }
    }

    void HipDropDown(){
        AngleChange(playerObj.transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime * 4f);
    }

    private void OnCollisionEnter(Collision other){
        if(CurrentState == State.hipDrop_Down && soarWaitTime <= 0){
            if(other.gameObject == playerObj){
                playerObj.GetComponent<PlayerCube>().CurrentHP -= 10;
                ChangeState(State.Idle);
            }
            if(other.gameObject.tag == "Floor" && soarWaitTime <= 0){
                ChangeState(State.Idle);
            }

        }
    }

    void ChangeStateRandom(){
        int val = Random.Range(0, 6);
        State i = (State)val;
        if(i == State.hipDrop_Down){
            ChangeStateRandom();
        }
        ChangeState(i);
    }

    void AngleChange(Vector3 target){
        Vector3 angle = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(angle);
    }
    
    void IdleMove(){
        transform.Translate(Vector3.forward * Time.deltaTime * (10f / moveSpeed));
    }

}
