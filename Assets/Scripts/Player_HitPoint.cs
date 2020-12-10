using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UIを使用する場合はこれが必要
using UnityEngine.UI;

public class Player_HitPoint : MonoBehaviour
{
    //Playerの現在HP
    public int HP;

    //Playerの最大HP;
    public int HPMax;

    //PlayerのHPバーに使用しているSlider
    public Slider P_Slider;

    void Start()
    {
        //SliderのmaxValue・現在HPを最大HPと同じ値にする
        P_Slider.maxValue = HPMax;
        HP = HPMax;
    }

    void Update()
    {
        //SliderのValueは現在HPと同じ
        P_Slider.value = HP;

        //HPの上限・下限
        if (HP <= 0) HP = 0;
        if (HP >= HPMax) HP = HPMax;        
    }
}
