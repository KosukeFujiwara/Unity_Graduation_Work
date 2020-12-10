using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{

    protected static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogError("SoundManager Instance Error");
                }
            }

            return instance;
        }
    }

    //音量
    public SoundVolume volume = new SoundVolume();

    //AudioSource 音を発生させるコンポーネント
    //BGM
    private AudioSource BGMsource;
    //ボイス
    private AudioSource VoiceSource;
    //SystemSE　メニュー周りで使用するシステム的なSE
    private AudioSource SystemSEsource;
    //GameSE　ゲームのメイン部分で使用するSE
    private AudioSource[] GamgeSEsources = new AudioSource[16];

    //必要があれば環境音などもあるといいかも

    //AudioClip 音源
    //BGM
    public AudioClip[] BGM;
    //ボイス
    public AudioClip[] Voice;
    //SystemSE
    public AudioClip[] SystemSE;
    //GameSE
    public AudioClip[] GameSE;


    void Awake()
    {
        if (this != Instance)
        {
            //既にオブジェクトが存在しているなら削除
            Destroy(gameObject);
            return;
        }
        //このゲームオブジェクトはシーンをまたいでも削除しない（DontDestroyOnLoad）
        DontDestroyOnLoad(gameObject);


        //全てのAudioSourceコンポーネントを追加する
        //BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        //BGMはループを有効にする
        BGMsource.loop = true;

        //音声プレイヤーと敵など複数の声が同時に発生するならAudioSourceを複数用意した方がいいかも
        //Voice AudioSource
        VoiceSource = gameObject.AddComponent<AudioSource>();

        //SystemSE AudioSource
        SystemSEsource = gameObject.AddComponent<AudioSource>();

        //GameSE AudioSource
        //GameSEは１つ１つAudioSourceに各AudioClipを割り当てる
        for (int i = 0; i < GamgeSEsources.Length; i++)
        {
            GamgeSEsources[i] = gameObject.AddComponent<AudioSource>();

            if (GameSE[i] != null)
            {
                GamgeSEsources[i].clip = GameSE[i];
            }
        }


        //ミュート設定
        BGMsource.mute      = volume.Mute;
        VoiceSource.mute    = volume.Mute;
        SystemSEsource.mute = volume.Mute;
        foreach (AudioSource source in GamgeSEsources)
        {
            source.mute = volume.Mute;
        }

        //ボリューム設定
        BGMsource.volume      = volume.BGM;
        VoiceSource.volume    = volume.Voice;
        SystemSEsource.volume = volume.SE;
        foreach (AudioSource source in GamgeSEsources)
        {
            source.volume = volume.SE;
        }
    }
    
    void Update()
    {
        //リアルタイムで音量を変えられる必要性は薄いので、
        //処理軽減のため、最終的にはUpdate関数内で音量を反映させる必要はない。

        //ミュート設定
        BGMsource.mute = volume.Mute;
        VoiceSource.mute = volume.Mute;
        SystemSEsource.mute = volume.Mute;
        foreach (AudioSource source in GamgeSEsources)
        {
            source.mute = volume.Mute;
        }

        //ボリューム設定
        BGMsource.volume      = volume.BGM;
        VoiceSource.volume    = volume.Voice;
        SystemSEsource.volume = volume.SE;
        foreach (AudioSource source in GamgeSEsources)
        {
            source.volume = volume.SE;
        }
    }

    //BGM再生
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index)
        {
            return;
        }
        //同じBGMの場合は何もしない
        if (BGMsource.clip == BGM[index])
        {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[index];
        BGMsource.Play();
    }

    //BGM停止
    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    //音声再生
    public void PlayVoice(int index)
    {
        if (0 > index || Voice.Length <= index)
        {
            return;
        }
        //「Play」の方法は音源の重複なしで再生
        VoiceSource.clip = Voice[index];
        VoiceSource.Play();
    }

    //音声停止
    public void StopVoice()
    {
        VoiceSource.Stop();
        VoiceSource.clip = null;
    }

    //システムSE再生
    public void PlaySysSE(int index)
    {
        if (0 > index || SystemSE.Length <= index)
        {
            return;
        }
        //「PlayOneShot」の方法は音源の重複ありで再生
        SystemSEsource.clip = SystemSE[index];
        SystemSEsource.PlayOneShot(SystemSEsource.clip);
    }

    //システムSE停止
    public void StopSysSE()
    {
        SystemSEsource.Stop();
        SystemSEsource.clip = null;
    }

    //ゲームSE再生
    public void PlayGameSE(int index)
    {
        if (0 > index || GameSE.Length <= index)
        {
            return;
        }
        //「PlayOneShot」の方法は音源の重複ありで再生
        GamgeSEsources[index].PlayOneShot(GamgeSEsources[index].clip);
    }

    //ゲームSE停止
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in GamgeSEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


}


//音量クラスも作成する。
//音量クラス
[System.Serializable] public class SoundVolume
{
    public bool Mute = false;
    [SerializeField, Range(0, 1), Tooltip("BGMの音量")]    public float BGM   = 1;
    [SerializeField, Range(0, 1), Tooltip("Voiceの音量")]  public float Voice = 1;
    [SerializeField, Range(0, 1), Tooltip("SEの音量")]     public float SE    = 1;

    public void Init()
    {
        BGM     = 1.0f;
        Voice   = 1.0f;
        SE      = 1.0f;
        Mute    = false;
    }
}

