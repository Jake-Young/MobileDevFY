using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Joystick m_JoystickElevationAndTurning;
    public float m_SpeedMultiplier = 3.0f;
    public float m_RotationMultiplier = 5.0f;

    // 0 => Stop, 1 => Full Speed
    private float m_Speed = 0.0f;
    // -1 => Lose Altitude, 1=> Gain Altitude
    private float m_Elevation = 0.0f;
    // Pos => Turn Right, Neg => Turn Left
    private float m_TurningSpeed = 0.0f;
    private bool m_EngineOn = false;

    private void Start()
    {
        m_JoystickElevationAndTurning = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EngineOn == true)
        {
            transform.Translate(Vector3.forward * (m_SpeedMultiplier * Time.deltaTime));
        }
        
        if (m_JoystickElevationAndTurning.Vertical >= 0.1f)
        {
            transform.Translate(Vector3.up * (m_SpeedMultiplier * Time.deltaTime));
        } 
        else if (m_JoystickElevationAndTurning.Vertical <= -0.1f) 
        {
            transform.Translate(Vector3.down * (m_SpeedMultiplier * Time.deltaTime));
        }

        transform.Rotate((-m_JoystickElevationAndTurning.Vertical * m_RotationMultiplier), 0.0f, (-m_JoystickElevationAndTurning.Horizontal * m_RotationMultiplier));
    }

    public void TurnEngineOnOff()
    {
        if (m_EngineOn == false)
        {
            m_EngineOn = true;
        } 
        else
        {
            m_EngineOn = false;
        }
    }

    public bool GetEngineOnOff()
    {
        return m_EngineOn;
    }
}
