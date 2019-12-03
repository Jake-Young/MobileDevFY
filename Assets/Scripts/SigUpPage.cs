using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigUpPage : MonoBehaviour
{

    [SerializeField]
    private GameObject m_ActivateMe;
    [SerializeField]
    private GameObject m_DeactivateMe;

    public void GoToActive()
    {
        m_ActivateMe.SetActive(true);
        m_DeactivateMe.SetActive(false);
    }
}
