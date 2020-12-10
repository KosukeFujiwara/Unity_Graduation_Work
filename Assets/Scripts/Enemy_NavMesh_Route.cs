using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_NavMesh_Route : MonoBehaviour
{
    //目標となるオブジェクト
    public GameObject[] Point;
    //目標地点のどこまで近づくかの距離
    public float PointDistance;
    //目標地点
    int NextPoint = 0;
    //NavMeshのコンポーネントを格納する変数
    NavMeshAgent Enemy_Nav;

    //NavMeshのSpeed（最高速）を代入する変数
    float Speed;
    //ダメージを受けて停止している時間
    public float DamageWaitTime;

    Animator animator;

    void Start()
    {
        //NavMeshAgentコンポーネントを取得
        Enemy_Nav = GetComponent<NavMeshAgent>();
        //最初に目標地点を設定
        GotoNextPoint();

        //NavMeshのSpeedを代入
        Speed = Enemy_Nav.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.MainPlay == true)
        {
            Enemy_Nav.enabled = true; //ナビメッシュ自体を有効に

            //現目標地点に近づいてきたら、次の目標地点を線t買う
            if (Enemy_Nav.pathPending == false && Enemy_Nav.remainingDistance < PointDistance)
            {
                GotoNextPoint();
            }
        }
        else
        {
            Enemy_Nav.enabled = false; //ナビメッシュ自体を無効に
        }
    }

    void GotoNextPoint()
    {
        //エージェントが現在設定された目標地点に行くように設定
        Enemy_Nav.destination = Point[NextPoint].transform.position;
        //配列内の次の位置を目標地点に設定し、必要ならば出発地点に戻す
        NextPoint = (NextPoint + 1) % Point.Length;
    }
    public void Damage()
    {
        //敵のSpeedを0にする
        //Enemy_Nav.speed = 0;

        //アニメーションを切り替え
        //animator.SetTrigger("Damage");

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
