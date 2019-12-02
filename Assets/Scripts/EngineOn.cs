using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineOn : MonoBehaviour
{

    public Image m_SoftShadow;

    private PlayerMovement m_Plane = null;
    private Button m_Button;
   
    // Start is called before the first frame update
    void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (m_Plane == null)
        {
            m_Plane = FindObjectOfType<PlayerMovement>();
        }

        m_Plane.TurnEngineOnOff();

        if (m_Plane.GetEngineOnOff())
        {
            Color32 green = new Color32(0, 189, 102, 255);

            m_Button.image.color = green;

            green.a = 60;
            m_SoftShadow.color = green;
        }
        else
        {
            Color32 red = new Color32(249, 38, 72, 255);

            m_Button.image.color = red;

            red.a = 60;
            m_SoftShadow.color = red;
        }
    }

}
