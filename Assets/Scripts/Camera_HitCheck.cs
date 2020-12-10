using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_HitCheck : MonoBehaviour
{
    //カメラをEditor上で指定できるように！
    public GameObject Camera;


    void Start()
    {
        //ゲーム機同時にカメラを非アクティブにしておく
        Camera.SetActive(false);
    }

    //isTriggerチェック時に呼ばれるもの

    //当たり判定にヒットしている間
    void OnTriggerStay(Collider trigger)
    {
       //「Tag」がPlayerのゲームオブジェクトにのみ処理を行う
       if(trigger.gameObject.tag == "Player")
        {
            Camera.SetActive(true); //対象のオブジェクトをアクティブに！
            Debug.Log("カメラ用のオブジェクトにヒットしているよ！");
        }
    }

    //当たり判定を通り抜け終えた瞬間
    private void OnTriggerExit(Collider trigger)
    {
        //「Tag」がPlayerのゲームオブジェクトにのみ処理を行う
        if(trigger.gameObject.tag == "Player")
        {
            Camera.SetActive(false);  //対象のオブジェクトを非アクティブに！
            Debug.Log("カメラのオブジェクトから離れたよ！");
        }
    }
}
