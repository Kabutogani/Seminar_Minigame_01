using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet02 : Bullet
{
    //弾の速度
    public float BulletSpeed;

    //拡散させる弾
    public GameObject DiffuseBullet;

    void FixedUpdate(){
        if(BulletSpeed >= 0f){
            Move();
            BulletSpeed = BulletSpeed - 4f * Time.deltaTime;
        }else{
            ScatterShot();
            Destroy(gameObject);    
        }
    }

    void Move(){
        transform.Translate(Vector3.forward * Time.deltaTime * BulletSpeed);
    }

    void ScatterShot(){
        Vector3 target = new Vector3(Random.Range(-20f,20f), 1f, Random.Range(-20f, 20f));

        for(int count = 0; count < 4; ++count){
            SetAngleShotForTarget(target, DiffuseBullet, 90*count);
        }
    }

    void SetAngleShotForTarget(Vector3 target, GameObject bulletType,float changeAngle){

        GameObject bullet = Instantiate(bulletType);

        bullet.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        Vector3 shootFor = new Vector3(target.x, 1f, target.z);
        bullet.transform.LookAt(shootFor);
        bullet.transform.Rotate(0, changeAngle, 0);
    }

    
}
