using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursorPointer : MonoBehaviour
{   

    public Vector3 mousePosition;
    public Vector3 mouse3DPosition;
    RaycastHit hitPoint;

    //プレイヤー
    public GameObject playerObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        //2D空間上のマウスの座標を取得
        mousePosition = Input.mousePosition;
        //2D空間上なのでZ軸が無い。そのため、なんか適当な数値を入れる。
        mousePosition.z = 2f;
        //マウス座標(yは適当)をWorldPointに変換
        mouse3DPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //カメラの座標からmouse3Dpositionに向かってレイを無限の長さ伸ばして、ヒットした場合、
        if(Physics.Raycast(Camera.main.transform.position, (mouse3DPosition - Camera.main.transform.position), out hitPoint, Mathf.Infinity)){
            //ゲームオブジェクトの位置をhitpointの位置に変える
            Vector3 mousePointerPoint = hitPoint.point;
            mousePointerPoint.y = mousePointerPoint.y + 0.2f;
            transform.position = mousePointerPoint;
        }

        //プレイヤーの方と逆の方向に向きを変える
        Vector3 playerVector = playerObject.transform.position - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(playerVector);
        transform.rotation = quaternion;
        //平面表示できるようにオブジェクトの角度を変える
        transform.localEulerAngles = new Vector3(90f, transform.localEulerAngles.y+180f, transform.localEulerAngles.z);

    }

}
