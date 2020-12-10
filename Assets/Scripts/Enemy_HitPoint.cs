using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UIを使用する場合はこれが必要
using UnityEngine.UI;

public class Enemy_HitPoint : MonoBehaviour
{
    //Enemyの現在HP
    public int HP;

    //Enemyの最大HP
    public int HPMax;

    //EnemyのHPバーに使用しているCanvas
    public GameObject E_Canvas;

    //EnemyのHPバーに使用しているSlider;
    public Slider E_Slider;

    //NavMeshを制御しているスクリプト用
    Enemy_NavMesh E_Nav;

    //ドロップするコインオブジェクトと数
    public GameObject Item;
    public int ItemNumber = 5;  
    

    void Start()
    {
        //SliderのmaxValue・現在HPを最大HPと同じに
        E_Slider.maxValue = HPMax;
        HP = HPMax;

        //NavMeshを制御しているスクリプトを取得
        E_Nav = GetComponent<Enemy_NavMesh>();
    }

    void Update()
    {
        //Canvasをカメラと同じ向きに設定
        E_Canvas.transform.rotation = Camera.main.transform.rotation;

        //SliderのValueは現在HPと同じ
        E_Slider.value = HP;
    }

    //当たり判定にヒットしている間
    void OnCollisionEnter(Collision collision)
    {
        //TagがBulletのゲームオブジェクトにのみ処理を行う
        if(collision.gameObject.tag == "Bullet")
        {
            //Speedを0にする処理へ
            E_Nav.Damage();

            //HP減少
            HP -= 1;

            //ダメージのエフェクト生成
            Instantiate(EffectManager.Instance.Enemy_Damage, collision.transform.position, transform.rotation);

            //HPが0以下になったら消滅
            if (HP <= 0)
            {
                //死亡時のエフェクト生成
                Instantiate(EffectManager.Instance.Enemy_Destroy, transform.position, transform.rotation);

                //設定した数だけアイテムをドロップ
                for(int i=0; i<ItemNumber; i++)
                {
                    Instantiate(Item, transform.position, Quaternion.Euler(90, 0, 0));
                }

                //自分自身を消す場合は()内をgameObjectに
                Destroy(gameObject);
            }
        }
    }
}
