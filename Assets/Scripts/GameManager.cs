using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  //TextMeshProを使う場合必須
using UnityEngine.Playables;  //TimeLineを使うときに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class GameManager : MonoBehaviour
{

    //シングルトン化
    protected static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if(instance == null)
                {
                    Debug.LogError("GameManeger Instance Error");
                }
            }
            return instance;
        }
    }

    //ゲーム中（操作可能）かどうかのフラグ
    public bool MainPlay;

    //制限時間
    public float TimeLimit = 60;

    //スコア
    public int Score = 0;

    //UI(TextMeshPro)に関する変数
    public TextMeshProUGUI TimeLimit_Text;
    public TextMeshProUGUI Score_Text;

    //TimeLine
    public PlayableDirector PD_GameOver;

    //GameOver時に表示するスコア・ハイスコア用
    public TextMeshProUGUI Result_Score_Text;
    public TextMeshProUGUI Result_HiScore_Text;
    int HiScore = 0;


    void Start()
    {
        //タイムの表示
        TimeLimit_Text.text = TimeLimit.ToString("0");

        //スコアの表示
        Score_Text.text = Score.ToString("00000000");

        //ゲーム開始のデモ演出作成後、この処理は別処理に変更する
        //------------------------------------------------------
        MainPlay = true;
        SoundManager.Instance.PlayBGM(0);
        //------------------------------------------------------
    }

    void Update()
    {
        //ゲーム中（操作可能）の時のみ処理を行う
        if(MainPlay == true)
        {
            //制限時間に関する処理
            if(TimeLimit > 0)
            {
                TimeLimit -= Time.deltaTime;
            }
            else
            {
                TimeLimit = 0;

                //GameOver時の処理開始
                GameOver();
            }

            //制限時間が残り10秒になったら、フォントのColorを変える(RGBaの順)
            if(TimeLimit < 10.0f)
            {
                TimeLimit_Text.color = new Color(1f, 0f, 0f, 0.9f);
            }
            else
            {
                //今後制限時間が回復することがあるなら10秒以上の時のColorも指定しておく
                TimeLimit_Text.color = new Color(1f, 1f, 1f, 0.9f);
            }

            //タイムの表示
            TimeLimit_Text.text = TimeLimit.ToString("0");
        }
    }

    public void CointCount()
    {
        //スコアをプラス
        Score += 1;

        //スコアの更新(表示)
        Score_Text.text = Score.ToString("00000000");
    }

    public void GameOver()
    {
        //操作不可にする
        MainPlay = false;

        //GameOver時のTimeLineを再生
        PD_GameOver.Play();
    }

    //GameOver時にスコアの表示、ハイスコア更新時に保存する処理
    //Timelineから呼び出す
    public void Score_Updata()
    {
        //保存していたHiScoreを一旦変数に格納
        HiScore = PlayerPrefs.GetInt("HiScore");
        if(Score > HiScore)
        {
            HiScore = Score;
            PlayerPrefs.SetInt("HiScore", HiScore);
            PlayerPrefs.Save();
        }
        
        //リザルトのスコアテキストを更新
        Result_Score_Text.text = Score.ToString("00000000");
        Result_HiScore_Text.text = HiScore.ToString("00000000");
    }

    //タイトル画面に戻るボタンを押したとき
    public void TitleBack()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);
        //フェードアウト
        FadeManager.Instance.LoadScene("Title", 1.0f);
    }

    //もう一度あそぶボタンを押したとき
    public void ReStart()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);
        //フェードアウト
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1.0f);
    }


}
