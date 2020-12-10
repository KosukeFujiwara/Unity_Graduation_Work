using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Controller : MonoBehaviour
{
    //自動追尾するターゲットオブジェクト(Player)
    GameObject Player;

    //自動追尾用のフラグ
    bool ChaseFLG;

    //親(Coin)オブジェクト
    GameObject Parent;

    //親(Coin)のRigidbodyとCollider用
    Rigidbody Parent_rigid;
    Collider Parent_coll;

    //Item_LayerオブジェクトのCollider用
    Collider Layer_coll;

    //自分(Item_Controller)のCollider用
    Collider My_coll;

    //コインが飛び出すときのランダムの範囲
    public float RandomRange = 40f;
    Vector3 RandomDirection;

    //親(Coin)の座標とターゲット(Player)の座標格納用
    Vector3 Parent_pos;
    Vector3 Target_pos;

    //アイテムの回転用
    Vector3 RotationAngle = new Vector3(0, 0, 1);
    public float RotationSpeed;

    //アイテムの入手までの時間をカウント
    public float WaitTime = 1.5f;


    void Start()
    {
        //Playerのオブジェクトを取得
        Player = GameObject.Find("Player");

        //自分の親オブジェクトを取得
        Parent = transform.parent.gameObject;

        //親オブジェクトのRigidbodyとColliderを取得
        Parent_rigid = Parent.GetComponent<Rigidbody>();
        Parent_coll = Parent.GetComponent<Collider>();

        //親から見て、0番目の子オブジェクト(Item_Layer)のColliderを取得
        Layer_coll = Parent.transform.GetChild(0).gameObject.GetComponent<Collider>();

        //自分のCollider
        My_coll = GetComponent<Collider>();

        //出現したときにランダムな方向へぴょんと飛び出してくる
        //最初からシーン上に配置する場合はRigidbodyの[is Kinematic]にチェックを入れ、動かないように
        RandomDirection = new Vector3(Random.Range(-RandomRange, RandomRange), 100 + RandomRange, Random.Range(-RandomRange, RandomRange));
        Parent_rigid.AddForce((RandomDirection), ForceMode.Impulse);
    }

    void Update()
    {
        //アイテムは常に回転している
        Parent.transform.Rotate(RotationAngle * RotationSpeed * Time.deltaTime);

        //自動追尾フラグがたったら
        if(ChaseFLG == true)
        {
            //追尾中に壁に引っかからないようItem_LayerオブジェクトのColliderも[is Trigger]をtrueに
            Layer_coll.isTrigger = true;

            //アイテムが自動追尾
            Target_pos = Player.transform.position;
            Target_pos.y = Player.transform.position.y + 2.5f;
            Parent_pos += (Target_pos - Parent.transform.position) * 5;

            Parent.transform.position += Parent_pos *= Time.deltaTime;
        }

        //敵からのドロップアイテムの場合(isKinematic = false)は出現してしばらくはアイテム取得できないように
        if(Parent_rigid.isKinematic == false)
        {
            //コルーチンを実行
            StartCoroutine("ItemWait");
        }
    }

    IEnumerator ItemWait()
    {
        //一定時間が経つまでは親のColliderと自分のColliderを非アクティブ化して入手できないように
        Parent_coll.enabled = false;
        My_coll.enabled = false;

        //指定した時間待つ
        yield return new WaitForSeconds(WaitTime);

        Parent_coll.enabled = true;
        My_coll.enabled = true;
    }

    //自動追尾するための当たり判定に接触したら
    public void OnTriggerEnter(Collider trigger)
    {
        //プレイヤーに接触したら追跡フラグを立てる
        if(trigger.gameObject.tag == "Player")
        {
            ChaseFLG = true;
        }
    }



}
