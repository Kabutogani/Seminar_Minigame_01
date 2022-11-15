using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet01 : Bullet
{

    //弾の速度
    public float BulletSpeed;

    void FixedUpdate(){
        Move();
    }

    void Move(){
        transform.Translate(Vector3.forward * Time.deltaTime * BulletSpeed);
    }
}
