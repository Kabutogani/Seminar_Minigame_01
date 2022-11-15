using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAttack : MonoBehaviour
{

    public int AttackPower;
    public float destoroyGrace = 5f;

    //移動速度
    public float magicMoveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destoroyGrace <= 0){
            Destroy(gameObject);
        }
        destoroyGrace = destoroyGrace - 1 * Time.deltaTime;

        //前に進む
        transform.Translate(Vector3.forward * magicMoveSpeed * Time.deltaTime);
    }
}
