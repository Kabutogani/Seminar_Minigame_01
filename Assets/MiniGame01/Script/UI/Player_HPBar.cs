using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HPBar : MonoBehaviour
{
    public GameObject playerObj;
    private PlayerCube player;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<PlayerCube>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        slider.value = (float)player.CurrentHP/(float)player.MaxHP;
    }
}
