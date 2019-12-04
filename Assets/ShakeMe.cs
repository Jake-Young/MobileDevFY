using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShakeMe : MonoBehaviour
{

    [SerializeField] private WeatherController m_WeatherController;
    private Vector3 m_MobileAcceleration;
    private int m_CycleCounter = 0;

    public TMP_Text m_ShakeTestLabel;
    public float m_Time = 5.0f; // 5 seconds

    void Update()
    {
        m_MobileAcceleration = Input.acceleration;

        m_ShakeTestLabel.text = string.Format("Shake: {0}", m_MobileAcceleration.sqrMagnitude);

        if (m_MobileAcceleration.sqrMagnitude >= 10.0f) 
        {
            if (m_CycleCounter == 0 && m_Time == 0.0f)
            {
                m_WeatherController.OnLondonClick();
                m_CycleCounter = 1;
                m_Time = 5.0f;
            }
            else if (m_CycleCounter == 1 && m_Time == 0.0f)
            {
                m_WeatherController.OnLosAngelesClick();
                m_CycleCounter = 2;
                m_Time = 5.0f;
            }
            else if (m_CycleCounter == 2 && m_Time == 0.0f)
            {
                m_WeatherController.OnMyLocationClick();
                m_CycleCounter = 0;
                m_Time = 5.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Time <= 0.0f)
        {
            m_Time = 0.0f;
        }
        else
        {
            m_Time -= Time.deltaTime;
        }
        
    }
}
