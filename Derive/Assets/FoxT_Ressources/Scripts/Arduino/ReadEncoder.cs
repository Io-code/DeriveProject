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
    public bool gouvTurned = false;
    [HideInInspector]
    public int fpTour = 0, spTour = 0;

    //Swim
    public float swimEncoderReadDelay;
    [HideInInspector] public float spSwimSpeed, fpSwimSpeed;
    public int fpLastEncoderValue, spLastEncoderValue;

    void Start()
    {
        UduinoManager.Instance.OnDataReceived += DataReveived;
    }

    private void Update()
    {
        if (gouvCurrentValue != gouvInitialeValue)
        {
            gouvInitialeValue = gouvCurrentValue;
            gouvTurned = true;
        }
        else gouvTurned = false;
        //Debug.Log(gouvTurned);
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
                Debug.Log("OK");
                if (fpCurrentValue >= fpInitialeValue + 40)
                {
                    fpInitialeValue = fpCurrentValue;
                    return 1;
                }
                else if (fpCurrentValue <= fpInitialeValue - 40)
                {
                    fpInitialeValue = fpCurrentValue;
                    return -1;
                }
            }
            else
            {
                if (spCurrentValue >= spInitialeValue + 40)
                {
                    spInitialeValue = spCurrentValue;
                    return 1;
                }
                else if(spCurrentValue <= spInitialeValue - 40)
                {
                    spInitialeValue = spCurrentValue;
                    return -1;
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
                fpSwimSpeed = value;
                fpLastEncoderValue = fpCurrentValue;
            }
            else
            {
                float value = Mathf.Abs(spCurrentValue - spLastEncoderValue);
                if (value < 0) value = 0;
                else if (value > 40) value = 40;
                value /= 40;
                spSwimSpeed = value;
                spLastEncoderValue = spCurrentValue;
            }
        }
    }
}
