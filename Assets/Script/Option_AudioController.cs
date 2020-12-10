using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Option_AudioController : MonoBehaviour
{
    //オプションメニューの親オブジェクト
    public GameObject OptionMenu;

    //ゲーム画面上のVolumeを変更するUI
    public Toggle Mute_Toggle;
    public Slider BGM_Slider;
    public Slider Voice_Slider;
    public Slider SE_Slider;

    //bool型はPlayerPrefsで保存できないので、
    //int型の変数でMuteの状態を変更する（0ならfalse、1ならtrue）
    int mute;

    void Start()
    {
        //オーディオの設定をロード
        SoundLoad();

    }

    void Update()
    {
        //オプションメニューがアクティブな時のみvalueの値を音量に適応
        if (OptionMenu.activeInHierarchy == true)
        {
            //SoundManagerのvolumeと各SliderのValueを同じに
            SoundManager.Instance.volume.Mute = Mute_Toggle.isOn;
            SoundManager.Instance.volume.BGM = BGM_Slider.value;
            SoundManager.Instance.volume.Voice = Voice_Slider.value;
            SoundManager.Instance.volume.SE = SE_Slider.value;
        }
    }

    //オーディオの設定を初期化---------------------------------------------
    public void SoundInit()
    {
        //ボリューム、ミュートをSliderに反映
        Mute_Toggle.isOn = SoundManager.Instance.volume.Mute;
        BGM_Slider.value = SoundManager.Instance.volume.BGM;
        Voice_Slider.value = SoundManager.Instance.volume.Voice;
        SE_Slider.value = SoundManager.Instance.volume.SE;

        //初期化後は設定を保存
        SoundSave();
    }

    //オーディオの設定をロード---------------------------------------------
    public void SoundLoad()
    {

        //サウンドの設定をロード
        mute = PlayerPrefs.GetInt("Mute");
        if (mute == 0)
        {
            SoundManager.Instance.volume.Mute = false;
        }
        else
        {
            SoundManager.Instance.volume.Mute = true;
        }

        SoundManager.Instance.volume.BGM = PlayerPrefs.GetFloat("BGMVolume");
        SoundManager.Instance.volume.Voice = PlayerPrefs.GetFloat("VoiceVolume");
        SoundManager.Instance.volume.SE = PlayerPrefs.GetFloat("SEVolume");

        //各SliderのValueをSoundManagerのvolumeと同じ値にする
        Mute_Toggle.isOn = SoundManager.Instance.volume.Mute;
        BGM_Slider.value = SoundManager.Instance.volume.BGM;
        Voice_Slider.value = SoundManager.Instance.volume.Voice;
        SE_Slider.value = SoundManager.Instance.volume.SE;
    }

    //オーディオの設定をセーブ---------------------------------------------
    public void SoundSave()
    {
        //Toggleの設定をmuteにも反映
        if(Mute_Toggle.isOn == false) { mute = 0; } else { mute = 1; }

        //サウンドの設定を保存
        PlayerPrefs.SetInt("Mute", mute);
        PlayerPrefs.SetFloat("BGMVolume", BGM_Slider.value);
        PlayerPrefs.SetFloat("VoiceVolume", Voice_Slider.value);
        PlayerPrefs.SetFloat("SEVolume", SE_Slider.value);
    }
}
