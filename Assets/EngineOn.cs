using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineOn : MonoBehaviour
{

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
    }

}
