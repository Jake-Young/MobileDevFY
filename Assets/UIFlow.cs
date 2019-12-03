using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using Firebase.Auth;

public class UIFlow : MonoBehaviour
{
    Regex m_EmailRegex = new Regex(@"(.com)|(.co.uk)\@");
    Regex m_PasswordRegex = new Regex(@"[^*_^%$£#~+=]");

    [SerializeField] private GameObject m_ErrorMesssagePanel;
    [SerializeField] private TMP_Text m_ErrorMessage;
    [SerializeField] private TMP_Text m_LoginEmail;
    [SerializeField] private TMP_Text m_RegisterEmail;
    [SerializeField] private TMP_InputField m_LoginPassword;
    [SerializeField] private TMP_InputField m_RegisterPassword;

    private string m_Password;
    private string m_Email;
    private bool m_EmailPass = false;
    private bool m_PasswordPass = false;

    public void CheckEmailIsValid(TMP_Text email)
    {
        Match registerEmailMatch = m_EmailRegex.Match(email.text.ToLower());

        if (registerEmailMatch.Success)
        {
            m_EmailPass = true;
            m_Email = email.text;
        } 
        else
        {
            m_ErrorMessage.text = "Email is not valid";
            m_ErrorMesssagePanel.SetActive(true);
            m_EmailPass = false;
        }
    }

    public void CheckPassWordIsValid(TMP_InputField password)
    {
        Match registerPasswordMatch = m_PasswordRegex.Match(password.text);

        m_Password = password.text;

        if (registerPasswordMatch.Success)
        {
            m_PasswordPass = true;
        }
        else
        {
            m_ErrorMessage.text = "Password is not valid";
            m_ErrorMesssagePanel.SetActive(true);
            m_PasswordPass = false;
        }
    }

    public void CheckPasswordMatch(TMP_InputField confirmPassword)
    {

        Match passwordMatch = Regex.Match(m_Password, confirmPassword.text);

        if (passwordMatch.Success)
        {
            m_PasswordPass = true;
        }
        else
        {
            m_ErrorMessage.text = "Passwords do not match";
            m_ErrorMesssagePanel.SetActive(true);
            m_PasswordPass = false;
        }
    }

    public void DismissErrorMessage()
    {
        m_ErrorMesssagePanel.SetActive(false);
    }

    public void LoginButtonClicked()
    {
        //StartCoroutine(RegisterUser(m_RegisterEmail.text, m_RegisterPassword.text));
    }

    public void RegisterButtonClicked()
    {
        if (m_EmailPass && m_PasswordPass)
        {
            m_EmailPass = false;
            m_PasswordPass = false;
            StartCoroutine(RegisterUser(m_Email, m_Password));
        }
        
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
        }
        else
        {
            Debug.Log($"Successfully registered user {registerTask.Result.Email}");
        }

    }
}
