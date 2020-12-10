using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //シングルトン化
    protected static EffectManager instance;
    public static EffectManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (EffectManager)FindObjectOfType(typeof(EffectManager));

                if(instance == null)
                {
                    Debug.LogError("EffectManagerのInstanceが存在しません！");
                }
            }
            return instance;
        }
    }

    //エフェクトのオブジェクト（プレハブ化したもの）をアサインする変数を用意

    //Player関連
    public GameObject Player_Jump;    //ジャンプ
    public GameObject Player_Damage;  //ダメージ

    //Bulelt関連
    public GameObject Bullet_Fire;    //発射
    public GameObject Bullet_Hit;     //着弾

    //Enemy関連
    public GameObject Enemy_Damage;   //ダメージ
    public GameObject Enemy_Destroy;  //死亡

    //Item関連
    public GameObject Item_CoinGet;   //コインゲット



}
