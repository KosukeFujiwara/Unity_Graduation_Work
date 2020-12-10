using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_BulletSpawn : MonoBehaviour
{
    //生成する弾丸のオブジェクト
    public GameObject Bullet;

    //弾丸のスピード
    public float BulletSpeed;

    //残弾数
    public int BulletCount = 300;

    //残弾数の上限
    public int BulletCountMax;

    //連射間隔
    public float Interval;

    //連射間隔のカウント
    private float IntervalCount;

    //弾丸の発射が可能かどうかのフラグ
    private bool FireFLG;

    //残弾数表示用のテキスト
    public TextMeshProUGUI BulletCount_Text;


    void Update()
    {
        //常にカウントを加算
        IntervalCount += Time.deltaTime;

        //カウントがIntervalを超えたら発射可能になる
        if (IntervalCount > Interval && FireFLG == false)
        {
            FireFLG = true;
        }

        //残弾数の表示
        BulletCount_Text.text = BulletCount.ToString("0000") + " / " + BulletCountMax.ToString("0000");
    }


    //弾丸を発射する処理
    //Player_Controllerでボタンを押す処理が実行されたらこの処理を行う
    public void BulletSpawn()
    {
        //発射可能状態でかつ残弾があれば弾丸を発射
        if(FireFLG == true && BulletCount > 0)
        {
            //弾丸消費
            BulletCount -= 1;

            //発射時のエフェクト生成
            Instantiate(EffectManager.Instance.Bullet_Fire, transform.position, transform.rotation);

            //生成した弾丸を変数に格納
            GameObject Bullets = Instantiate(Bullet, transform.position, transform.rotation);

            //格納した弾丸のRigidBodyを取得
            Rigidbody BulletRig = Bullets.GetComponent<Rigidbody>();

            //弾丸に瞬発的な力を加える
            BulletRig.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);

            //発射した弾丸オブジェクトは3秒後に破壊する
            Destroy(Bullets, 3.0f);

            //フラグをfalseにし、カウントをリセット
            FireFLG = false;
            IntervalCount = 0;
        }
    }

    //弾丸をリロード
    public void BulletReload()
    {
        //残弾数をMaxの値に
        BulletCount = BulletCountMax;
    }
}
