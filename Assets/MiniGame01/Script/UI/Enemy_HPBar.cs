using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPBar : MonoBehaviour
{
    public GameObject enemyObj;
    private Enemy enemy;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        enemy = enemyObj.GetComponent<Enemy>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        slider.value = (float)enemy.currentHP/(float)enemy.maxHP;
    }
}
