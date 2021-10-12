using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ReadEncoder : MonoBehaviour
{
    private int fpInitialeValue, fpCurrentValue;
    private int spInitialeValue, spCurrentValue;
    public int fpTour = 0, spTour = 0;

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
}
