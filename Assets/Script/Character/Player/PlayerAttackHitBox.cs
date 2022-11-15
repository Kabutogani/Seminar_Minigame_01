using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitBox : MonoBehaviour
{   

    public int AttackPower;
    public float destoroyGrace = 0.2f;

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
    }
}
