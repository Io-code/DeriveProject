using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Uduino;
using System;

public class ReadEncoder : MonoBehaviour
{
    private int fpInitialeValue, fpCurrentValue;
    private int spInitialeValue, spCurrentValue;
    private int gouvInitialeValue, gouvCurrentValue;
    [HideInInspector]
    public int fpTour = 0, spTour = 0;

    //Swim
    public float swimEncoderReadDelay;
    [HideInInspector] public float swimSpeed;
    public int fpLastEncoderValue, spLastEncoderValue;

    void Start()
    {
        UduinoManager.Instance.OnDataReceived += DataReveived;
    }

    void DataReveived(string data, UduinoDevice board)
    {
        string dataTemp = "";
        for (int i = 2; i < data.Length; i++)
        {
            dataTemp += data[i];
        }
        if (data[0] == '1')
        {
            fpCurrentValue = int.Parse(dataTemp);
        }
        else if (data[0] == '2')
        {
            spCurrentValue = int.Parse(dataTemp);
        }
        else
        {
            gouvCurrentValue = int.Parse(dataTemp);
        }
    }
    public void StartRead(bool firstPlayer)
    { 
        if (firstPlayer) fpInitialeValue = fpCurrentValue;
        else spInitialeValue = spCurrentValue;    
    }

    public int UpdateRead(bool firstPlayer)
    {
            if (firstPlayer)
            {
                if (fpCurrentValue >= fpInitialeValue + 80)
                {
                    return 1;
                    fpInitialeValue = fpCurrentValue;
                }
                else if (fpCurrentValue <= fpInitialeValue - 80)
                {
                    return -1;
                    fpInitialeValue = fpCurrentValue;
                }
            }
            else
            {
                if (spCurrentValue >= spInitialeValue + 80)
                {
                    return 1;
                    spInitialeValue = spCurrentValue;
                }
                else if(spCurrentValue <= spInitialeValue - 80)
                {
                    return -1;
                    spInitialeValue = spCurrentValue;
                }
            }
        return 0;
    }

    public IEnumerator SwimRead(byte firstPl)
    {
        fpLastEncoderValue = fpCurrentValue;
        spLastEncoderValue = spCurrentValue;
        while(true)
        { 
            yield return new WaitForSeconds(swimEncoderReadDelay);
            if (firstPl == 1)
            {
                float value = Mathf.Abs(fpCurrentValue - fpLastEncoderValue);
                if (value < 0) value = 0;
                else if (value > 40) value = 40;
                value /= 40;
                swimSpeed = value;
                fpLastEncoderValue = fpCurrentValue;
            }
            else
            {
                swimSpeed = Mathf.Clamp(Mathf.Abs(spCurrentValue - fpLastEncoderValue), 0, 40) / 40;
                spLastEncoderValue = spCurrentValue;
            }
        }
    }
}
