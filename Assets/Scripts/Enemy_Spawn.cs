using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    //スポーンさせる敵オブジェクト
    public GameObject SpawnEnemy;

    //敵の出現時間（ランダムにするのでその最小値と最大値）
    public float interval_min = 3;
    public float interval_max = 5;

    float RandomCount;
    float interval_Count;
    void Start()
    {
        //ランダムな値を取得
        RandomCount = Random.Range(interval_min, interval_max);
    }

    void Update()
    {
        if(GameManager.Instance.MainPlay == true)
        {
            //時間をカウント
            interval_Count += Time.deltaTime;

            if (interval_Count > RandomCount)
            {
                //敵をインスタンス化
                //第一引数でオブジェクトを指定、第二引数でposition、第三引数でrotationを指定できる
                Instantiate(SpawnEnemy, transform.position, transform.rotation);

                //時間をリセット
                interval_Count = 0;

                //再度ランダムな値を取得
                RandomCount = Random.Range(interval_min, interval_max);
            }
        }
    }
}
