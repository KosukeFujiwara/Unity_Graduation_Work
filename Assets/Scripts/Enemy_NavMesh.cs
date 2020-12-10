using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_NavMesh : MonoBehaviour
{

    //目標となるオブジェクト
    public GameObject Target;

    //NavMeshのコンポーネントを格納する変数
    NavMeshAgent Enemy_Nav;

    //NavMeshのSpeed（最高速）を代入する変数
    float Speed;

    //ダメージを受けて停止している時間
    public float DamageWaitTime;

    //アニメーション
    Animator animator;


    void Start()
    {
        //NavMeshAgentコンポーネントを取得
        Enemy_Nav = GetComponent<NavMeshAgent>();

        //インスタンス化の際、ターゲットとなるオブジェクトを直接指定
        if (Target == null)
        {
            Target = GameObject.Find("Player");
        }

        //NavMeshのSpeedを代入
        Speed = Enemy_Nav.speed;

        //Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

    }
    void Update()
    {
        if (GameManager.Instance.MainPlay == true)
        {
            Enemy_Nav.enabled = true; //ナビメッシュ自体を有効に

            //NavMeshAgentに目的地をセット
            Enemy_Nav.SetDestination(Target.transform.position);
        }
        else
        {
            Enemy_Nav.enabled = false; //ナビメッシュ自体を無効に
        }      
    }

    public void Damage()
    {
        //敵のSpeedを0にする
        Enemy_Nav.speed = 0;

        //アニメーションを切り替え
        animator.SetTrigger("Damage");

        //コルーチンを実行
        StartCoroutine("DamageWait");
    }

    IEnumerator DamageWait()
    {
        //指定した時間待つ
        yield return new WaitForSeconds(DamageWaitTime);

        //弾丸を受けたときに0にされたSpeedを戻す
        Enemy_Nav.speed = Speed;
    }
}
