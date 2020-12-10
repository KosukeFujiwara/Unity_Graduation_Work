using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シーンのロードに必要
using UnityEngine.SceneManagement;

//TextMeshProを表示するのに必要
using TMPro;

public class DebugManager : MonoBehaviour
{
    //Playerの移動速度表示
    public TextMeshProUGUI PlayerSpeed;

    //Playerオブジェクトからスクリプトを取得するための変数
    public GameObject Player;
    Player_Controller P_Controller;
    Player_HitPoint P_HP;

    void Start()
    {
        //PlayerオブジェクトからPlayerControllerのスクリプト(Conponent)を取得
        P_Controller = Player.GetComponent<Player_Controller>();
        P_HP = Player.GetComponent<Player_HitPoint>();
    }

    void Update()
    {
        //デバッグ用ボタンを押したら今のシーンを再読み込み
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneLoad_Debug();
            Debug.Log("Pを押したよ！");
        }

        //Playerの移動速度表示
        //PlayerSpeed.text = "PlayerMoveSpeed = " + P_Controller.P_Rig.velocity.magnitude.ToString("000");

        //デバッグ用//ボタンを押したらHPをMAXにする
        if (Input.GetKeyDown(KeyCode.I))
        {
            P_HP.HP = P_HP.HPMax;
            Debug.Log("HPを回復したよ！");
        }

        //デバッグ用//ボタンを押したらTimeを0にする！
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.TimeLimit = 0;
            Debug.Log("Timeを0にしたよ！");
        }
    }

    public void SceneLoad_Debug()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("シーンを再読み込みしたよ！");
    }

}
