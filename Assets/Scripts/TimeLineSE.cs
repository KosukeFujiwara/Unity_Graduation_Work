using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineSE : MonoBehaviour
{
    //自身のAudioSource
    private AudioSource Source;

    void Start()
    {
        //自身のAudioSource
        Source = gameObject.GetComponent<AudioSource>();        
    }

    void Update()
    {
        //自身のAudioSourceのmuteとvolumeをSoundManagerと同じに。
        //リアルタイムで音量変更しないのならUpdate関数内でなくてもよい。
        Source.mute = SoundManager.Instance.volume.Mute;
        Source.volume = SoundManager.Instance.volume.SE;
    }
}
