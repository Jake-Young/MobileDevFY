using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using Firebase.Auth;

public class UIFlow : MonoBehaviour
{
    Regex m_EmailRegex = new Regex(@"(.com)|(.co.uk)\@");
    Regex m_PasswordRegex = new Regex(@"[^*_^%$£#~+=]");

    [SerializeField] private GameObject m_MesssagePanel;
    [SerializeField] private TMP_Text m_Message;

    private string m_Password;
    private string m_Email;
    private string m_StringFormat = "User: {0}";
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
            m_Message.text = "Email is not valid";
            m_MesssagePanel.SetActive(true);
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
            m_Message.text = "Password is not valid";
            m_MesssagePanel.SetActive(true);
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
            m_Message.text = "Passwords do not match";
            m_MesssagePanel.SetActive(true);
            m_PasswordPass = false;
        }
    }

    public void DismissErrorMessage()
    {
        m_MesssagePanel.SetActive(false);
    }

    public void LoginButtonClicked()
    {
        if (m_EmailPass && m_PasswordPass)
        {
            m_EmailPass = false;
            m_PasswordPass = false;
            StartCoroutine(SignInUser(m_Email, m_Password));
        }
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
            StartCoroutine(LoadAsyncScene(1));
        }

    }

    private IEnumerator SignInUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var signInTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.Exception != null)
        {
            Debug.LogWarning($"Failed to sign in with {signInTask.Exception}");
        }
        else
        {
            Debug.Log($"Successfully signed in user {signInTask.Result.Email}");
            StartCoroutine(LoadAsyncScene(1));
        }
    }

    private IEnumerator LoadAsyncScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        yield return new WaitUntil(() => asyncLoad.isDone);
    }
}
