using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using System;

public class LoadSceneWhenUserAuthenticated : MonoBehaviour
{

    [SerializeField] private int m_SceneToLoad;

    private void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckUser();
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        CheckUser();
    }

    private void CheckUser()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene(m_SceneToLoad);
        }
    }
}
