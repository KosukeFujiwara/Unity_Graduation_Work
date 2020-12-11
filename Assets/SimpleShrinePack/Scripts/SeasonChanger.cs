using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonChanger : MonoBehaviour
{
    public float seasonCounter;
    public float duration = 2f;
    float currentTime = 0f;
    public Material leavesMat1;
    public Material leavesMat2;
    public Material grassMat;
    public Color Summer1;
    public Color Summer2;
    public Color Autumn1;
    public Color Autumn2;
    public Color Winter1;
    public Color Winter2;
    public Color Spring1;
    public Color Spring2;
    public Color GrassSummer;
    public Color GrassWinter;

    
    //This function can be called to switch to the next season
    public void changeSeason ()
    {
        seasonCounter += 1;
    }
    public void Update () {
        
    //seasonCounter float cycling through 1 to 4, 1 being spring and 4 being Winter. If you want to skip a season you can directly access this float and set it to your desired season (Numbers 1 through 4)
        if (seasonCounter >= 5){
            seasonCounter = 1;
        }


    //Setting season colours by Lerping the material color from current color to wanted season color. Duration can be changed through the "duration" float (has to be higher than 0).
    //You can also change the specific season colors in the Seasonchanger Prefab
    //Grass Color (m3) changes in Spring and winter, if you want it to change every season you can copy paste "m3.color = Color.Lerp(m3.color, GrassSummer, currentTime / duration);" into each seasons respective bracket with different colors.

        if (seasonCounter == 1)
        { 
            currentTime = 0f;
            bool changingSeason = true;
            if (currentTime < duration && changingSeason == true)
            {
            currentTime += Time.deltaTime;
            leavesMat1.color = Color.Lerp(leavesMat1.color, Spring1, currentTime / duration);
            leavesMat2.color = Color.Lerp(leavesMat2.color, Spring2, currentTime / duration);
            grassMat.color = Color.Lerp(grassMat.color, GrassSummer, currentTime / duration);
            } else {
                currentTime = 0;
                changingSeason = false;
            }
            
        }
        if (seasonCounter == 2)
        {
            currentTime = 0f;
            bool changingSeason = true;
            if (currentTime < duration && changingSeason == true)
            {
            currentTime += Time.deltaTime;
            leavesMat1.color = Color.Lerp(leavesMat1.color, Summer1, currentTime / duration);
            leavesMat2.color = Color.Lerp(leavesMat2.color, Summer2, currentTime / duration);
            } else {
                currentTime = 0;
                changingSeason = false;
            }
        }
        if (seasonCounter == 3)
        {
            currentTime = 0f;
            bool changingSeason = true;
            if (currentTime < duration && changingSeason == true)
            {
            currentTime += Time.deltaTime;
            leavesMat1.color = Color.Lerp(leavesMat1.color, Autumn1, currentTime / duration);
            leavesMat2.color = Color.Lerp(leavesMat2.color, Autumn2, currentTime / duration);
            } else {
                currentTime = 0;
                changingSeason = false;
            }
        }
        if (seasonCounter == 4)
        {
            currentTime = 0f;
            bool changingSeason = true;
            if (currentTime < duration && changingSeason == true)
            {
            currentTime += Time.deltaTime;
            leavesMat1.color = Color.Lerp(leavesMat1.color, Winter1, currentTime / duration);
            leavesMat2.color = Color.Lerp(leavesMat2.color, Winter2, currentTime / duration);
            grassMat.color = Color.Lerp(grassMat.color, GrassWinter, currentTime / duration);
            } else {
                currentTime = 0;
                changingSeason = false;
            }
        }
    }
}
