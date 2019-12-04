using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class CurrentUserLabel : MonoBehaviour
{

    [SerializeField] private TMP_Text m_UserLabel;
    private string m_FormatString;

    private void Awake()
    {
        m_UserLabel = GetComponent<TMP_Text>();
        m_FormatString = "User: {0}";
    }

    private void Start()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        string username;
        if (currentUser == null)
        {
            username = "NULL";
        }
        else
        {
            username = currentUser.Email;
        }

        Debug.Log($"Usermame: {username}");

        m_UserLabel.text = string.Format(m_FormatString, username);
    }
}
