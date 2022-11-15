using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{      
    //プレイヤーと敵のどっちがもってるのか
    public enum HaveSide{Player, Enemy}

    //弾の動き方の種類
    public enum BulletType{Straight, AutoTrack}

    //弾のダメージ
    public int bulletDamage = 0;

    //これのリジッドボディ
    Rigidbody rb;

    public HaveSide haveSide;

    //自動消滅までの時間
    public float lifeTime = 10f;

    //プレイヤーのオブジェクト
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        switch(haveSide) {
            case HaveSide.Player:
                if(other.gameObject.GetComponent<Enemy>() != null){
                    AddDamageForEnemy(bulletDamage, other.gameObject.GetComponent<Enemy>());
                }
            break;

            case HaveSide.Enemy:
                if(other.gameObject.GetComponent<PlayerCube>() != null){
                    AddDamageForPlayer(bulletDamage, other.gameObject.GetComponent<PlayerCube>());
                }
            break;
        }
    }

    void AddDamageForEnemy(int damage, Enemy enemy){
        if(enemy.currentHP > 0){
            enemy.currentHP -= damage;
        }else{
            enemy.Death();
        }

        Destroy(gameObject);
        
    }

    void AddDamageForPlayer(int damage, PlayerCube player){
        if(player.CurrentHP > 0){
            if(player.isGuard){
                player.CurrentHP = player.CurrentHP - (damage/2);
            }
            else{
                player.CurrentHP = player.CurrentHP - damage;
            }

        }else{
            player.Death();
        }
        Destroy(gameObject);
    }

}
