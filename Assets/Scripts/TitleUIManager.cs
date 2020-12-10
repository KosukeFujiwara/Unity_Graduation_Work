using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUIManager : MonoBehaviour
{
    //各メニュー用UIのオンオフ用
    public GameObject MainmenuUI;
    public GameObject OptionMenuUI;
    public GameObject OperationUI;

    //ハイスコア格納用
    int HiScore;
    public TextMeshProUGUI HiScore_Text;

    //オーディオの設定をしているスクリプト
    Option_AudioController AudioControll;


    void Start()
    {
        AudioControll = GetComponent<Option_AudioController>();

        //ゲーム開始直後はMainMenuのみ表示
        MainmenuUI.SetActive(true);
        OptionMenuUI.SetActive(false);
        OperationUI.SetActive(false);

        //ハイスコアのロードと表示
        HiScoreLoad();

        //オーディオの設定をロード
        AudioControll.SoundLoad();

        //BGM再生
        SoundManager.Instance.PlayBGM(0);
    }

    //ゲーム開始ボタンを押したとき------------------------------------
    public void GameStart()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);

        //シーン名を指定して、そのシーンに遷移する
        //必ずBuildSettingsでシーンを追加していること！
        //SceneManager.LoadScene("MainGame");

        FadeManager.Instance.LoadScene("MainGame", 1.0f); //第二引数でフェード秒数
    }

    //オプションボタンを押したとき------------------------------------
    public void Option()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);

        //MainMenuを非アクティブにし、OptionMenuをアクティブに
        MainmenuUI.SetActive(false);
        OptionMenuUI.SetActive(true);
    }

    //操作説明ボタンを押したとき-------------------------------------
    public void Operation()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);

        //MainMenuを非アクティブにし、OperationUIをアクティブに
        MainmenuUI.SetActive(false);
        OperationUI.SetActive(true);
    }

    //戻るボタンを押したとき（オプション・操作説明共通）-------------
    public void MenuBack()
    {
        //SE
        SoundManager.Instance.PlaySysSE(2);

        //MainMenuを非アクティブにし、他のUIを非アクティブに
        MainmenuUI.SetActive(true);
        OptionMenuUI.SetActive(false);
        OperationUI.SetActive(false);

        //オーディオの設定をセーブ
        AudioControll.SoundSave();
    }

    //データの初期化ボタンを押したとき-------------------------------
    public void Initialized()
    {
        //SE
        SoundManager.Instance.PlaySysSE(0);

        //PlayPrefsで保存した全データを削除
        PlayerPrefs.DeleteAll();

        //ハイスコアのロードと表示
        HiScoreLoad();

        //SoundManagerのMute、Volumeを初期化
        SoundManager.Instance.volume.Init();

        //オーディオの設定をセーブ
        AudioControll.SoundInit();
    }

    //ゲーム終了ボタンを押したとき-----------------------------------
    public void Exit()
    {
        //UnityEditor上でプレイを終了する場合
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #else
        //ビルドした実行データでプレイを終了する場合
            Application.Quit();
        #endif
    }

    //ハイスコアのロードと表示---------------------------------------
    public void HiScoreLoad()
    {
        //保存していたHiScoreを一旦変数に格納
        HiScore = PlayerPrefs.GetInt("HiScore");
        //ハイスコアのテキストを表示
        HiScore_Text.text = "HiScore : " + HiScore.ToString("00000000");
    }
}
