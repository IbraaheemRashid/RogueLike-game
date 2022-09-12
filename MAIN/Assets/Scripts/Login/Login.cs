using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public string Username;
    public string Password;
    private string[] Lines;
    private string DecryptedPW;

    public void LoginButton()
    {
        bool UN = false;
        bool PW = false;
        if(Username != "")
        {
            if(System.IO.File.Exists(@"C:/UnityLogins/" + Username + ".txt"))
            {
                UN = true;
                Lines = System.IO.File.ReadAllLines(@"C:/UnityLogins/" + Username + ".txt");
            }
            else
            {
                Debug.LogWarning("Username not found");
            }
        }
        else
        {
            Debug.LogWarning("Username Field Empty");
        }
        if(Password != "")
        {
            if(System.IO.File.Exists(@"C:/UnityLogins/" + Username + ".txt"))
            {
                int i = 1;
                foreach (char c in Lines[2])
                {
                    i++;
                    char Descrypted = (char)(c / i);
                    DecryptedPW += Descrypted.ToString();
                }
                if(Password == DecryptedPW)
                {
                    PW = true;
                }
                else
                {
                    Debug.LogWarning("Password is invalid");
                }
            }
            else
            {
                Debug.LogWarning("Password is invalid/Doesn't exist");
            }
        }
        else
        {
            Debug.LogWarning("Password Field Empty");
        }
        if(UN == true && PW == true)
        {
            username.text = "";
            password.text = "";
            print("Login Successful");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                password.Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Password != "" && Username != "")
            {
                LoginButton();
            }
        }
        Username = username.text;
        Password = password.text;
        }
    }

