using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Player_HitCheck : MonoBehaviour
{
    //Player_Controller
    Player_Controller P_Controll;

    //Player_HitPointスクリプト
    Player_HitPoint P_HP;

    //衝突した座標を入れる変数
    Vector3 hitPos;

    //ノックバックの強さ
    public float KBPower;

    //ノックバックしている時間
    public float KBTime;

    //public bool MainPlay;
    //public PlayableDirector PD_GameOver;

    //NavMeshAgent Enemy_Nav;

    void Start()
    {
        //GetComponentでPlayerオブジェクトの他のスクリプトを取得
        P_Controll = GetComponent<Player_Controller>();
        P_HP = GetComponent<Player_HitPoint>();

        //MainPlay = true;

        //Enemy_Nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        //Enemy_Nav.enabled = true; //ナビメッシュ自体を有効に
    }

    //当たり判定にヒットしている間
    private void OnCollisionEnter(Collision collision)
    {
 
            //「Tag」がEnemyのゲームオブジェクトにのみ処理を行う
            if(collision.gameObject.tag == "Enemy")
            {
                if (P_Controll.Invincible == false)
                {
                    //一旦プレイヤーの移動量を0にした後、敵の向いてる方向にノックバックの力を加える
                    P_Controll.P_Rig.velocity = Vector3.zero;
                    P_Controll.P_Rig.AddForce(collision.transform.rotation * new Vector3(0, 0, KBPower), ForceMode.VelocityChange);

                    //コルーチンを実行するためにはStartCoroutineメソッドを使う
                    StartCoroutine("KnockBack");

                    //衝突オブジェクトを消す。自分自身お消す場合は()の中をgameObjectに！
                    Destroy(collision.gameObject);

                    //敵とぶつかったらダメージを受ける
                    P_HP.HP -= 1;
                    Debug.Log("ダメージを受けた！");

                    //foreachで衝突した当たり判定の座標を取得
                    foreach(ContactPoint point in collision.contacts)
                    {
                        hitPos = transform.InverseTransformPoint(point.point);

                        //ダメージ時のエフェクト生成
                        Instantiate(EffectManager.Instance.Player_Damage, transform.position + hitPos, transform.rotation);
                    }
                    if (P_HP.HP <= 0)
                    {
                        GameManager.Instance.GameOver();
                    }
                }
            }
     }

    IEnumerator KnockBack()
    {
        //無敵状態に
        P_Controll.Invincible = true;

        //指定した時間待つ
        yield return new WaitForSeconds(KBTime);

        //無敵状態を解除
        P_Controll.Invincible = false;
    }

    void OnTriggerEnter(Collider trigger)
    {
        //TagがCoinのゲームオブジェクトにのみ処理を行う
        if(trigger.gameObject.tag == "Coin")
        {
            //SE
            SoundManager.Instance.PlayGameSE(3);

            //Effect
            Instantiate(EffectManager.Instance.Item_CoinGet, trigger.transform.position, Quaternion.identity);

            //GameManagerのスコアを加算する処理へ
            GameManager.Instance.CointCount();

            //コインのオブジェクトを消す
            Destroy(trigger.gameObject);
        }
    }
}
