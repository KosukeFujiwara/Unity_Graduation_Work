using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpCheck : MonoBehaviour
{
    //地面に接地しているかどうかのフラグ
    //[System.NonSerialized] public bool isGround;
    public bool isGround;

    void OnTriggerStay(Collider trigger)
    {
        //Groundタグのオブジェクトに接触していたら
        if(trigger.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    void OnTriggerExit(Collider trigger)
    {
        //Groundタグのオブジェクトから離れたら
        if(trigger.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }
}

