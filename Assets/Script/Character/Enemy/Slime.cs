using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    //最大体力
    public int MaxHp;
    //現在体力
    public int CurrentHp;
    //攻撃された後の無敵時間
    public float invisibleTime;
    //攻撃された後の無敵時間の最大値
    public float maxInvisibleTime = 0.5f;
    //死んでから消えるまでの時間
    public float DestroyTime = 3f;
    
    //攻撃を受けることが可能か？
    public bool canHitDamage;
    //死んではいないか？
    public bool isDeath;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;
        canHitDamage = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        //死亡判定
        if(isDeath == false){

            if(invisibleTime <= 0) {

                canHitDamage = true;
                invisibleTime = 0;

            } else {

                canHitDamage = false;
                invisibleTime = invisibleTime - 1 * Time.deltaTime;

            }
            if(CurrentHp <= 0){
                Death();
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        GameObject obj;
        obj = other.gameObject;
        if(obj.GetComponent<PlayerAttackHitBox>() != null && canHitDamage == true){

            Damage(obj.GetComponent<PlayerAttackHitBox>().AttackPower);
            
        }
        if(obj.GetComponent<PlayerMagicAttack>() != null && canHitDamage == true){

            Damage(obj.GetComponent<PlayerMagicAttack>().AttackPower);
            Destroy(obj);
            
        }
        Debug.Log(obj.name);
    }

    void Damage(int damage) {

        if(damage > 0){

            //ダメージを受ける
            CurrentHp = CurrentHp - damage;

            if(CurrentHp > 0){
            //死んでないなら無敵時間の発生
            invisibleTime = maxInvisibleTime;
            }

        }
    }

    void Death() {
        if(isDeath == false){
            //ダメージを受けれない状態にする
            canHitDamage = false;
            //死亡アニメーションを流す
            animator.SetTrigger("Death");
            //死んだことにする
            isDeath = true;
        } else {
            if(DestroyTime >= 0){
                DestroyTime = DestroyTime - 1 * Time.deltaTime;
            }else {
                Destroy(gameObject);
            }
        }
    }
}
