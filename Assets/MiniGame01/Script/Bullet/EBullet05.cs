using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBullet05 : Bullet
{
    //弾の速度
    public float BulletSpeed;
    public float WatchSpeed;

    void FixedUpdate(){
        Move();
        AngleChange();
    }

    void Move(){
        transform.Translate(Vector3.forward * Time.deltaTime * BulletSpeed);
    }

    void AngleChange(){
        // float y;
        // if(playerObj.transform.position.y <= transform.position.y){
        //     y = WatchSpeed;
        // }else{
        //     y = -WatchSpeed;
        // }
        // transform.Rotate(0,y,0);
        Vector3 target = playerObj.transform.position;
        target.y = 1;
        transform.LookAt(target);
    }
}
