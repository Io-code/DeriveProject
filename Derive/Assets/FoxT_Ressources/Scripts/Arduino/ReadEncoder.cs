using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Uduino;

public class ReadEncoder : MonoBehaviour
{
    private int fpInitialeValue, fpCurrentValue;
    private int spInitialeValue, spCurrentValue;
    [HideInInspector]
    public int fpTour = 0, spTour = 0;

    //Swim
    public float swimEncoderReadDelay;
    public float swimSpeed;
    public int lastEncoderValue;

    void Start()
    {
        UduinoManager.Instance.OnDataReceived += DataReveived;
    }

    void DataReveived(string data, UduinoDevice board)
    {
        fpCurrentValue = int.Parse(data);
    }

    public IEnumerator StartRead(float duration, bool firstPlayer)
    {
        fpInitialeValue = fpCurrentValue;
        spInitialeValue = spCurrentValue;
        float timeElapsed = 0;
        while (timeElapsed <= duration)
        {
            yield return new WaitForEndOfFrame();
            fpTour = 0;
            spTour = 0;
            timeElapsed += Time.deltaTime;
            if (firstPlayer)
            {
                if (fpCurrentValue >= fpInitialeValue + 80)
                {
                    fpTour = 1;
                    fpInitialeValue = fpCurrentValue;
                }
                else if (fpCurrentValue <= fpInitialeValue - 80)
                {
                    fpTour = -1;
                    fpInitialeValue = fpCurrentValue;
                }
            }
            else
            {
                if (spCurrentValue >= spInitialeValue + 80)
                {
                    spTour = 1;
                    spInitialeValue = spCurrentValue;
                }
                else if(spCurrentValue <= spInitialeValue - 80)
                {
                    spTour = -1;
                    spInitialeValue = spCurrentValue;
                }
            }
        }
    }

    public IEnumerator SwimRead(MonoBehaviour owner, bool firstPl)
    {
        while(true)
        { 
            yield return new WaitForSeconds(swimEncoderReadDelay);
            if (firstPl)
            {
                swimSpeed = Mathf.Clamp(fpCurrentValue - lastEncoderValue, -40, 40) / 40;
            }
            else swimSpeed = Mathf.Clamp((spCurrentValue - lastEncoderValue), -40, 40) / 40;
        }
    }
}
