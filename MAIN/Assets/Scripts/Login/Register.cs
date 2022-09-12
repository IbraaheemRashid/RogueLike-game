using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;


public class Register : MonoBehaviour
{
    public InputField username;
    public InputField email;
    public InputField password;
    public InputField confPassword;
    private string Username;
    private string Email;
    private string Password;
    private string ConfPassword;
    private string form;
    private bool EmailValid = false;
    private string[] Characters = {"a", "b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z", //creates a list of all the characters that can be used in email
                                   "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                   "1","2","3","4","5","6","7","8","9","0","_","-"};

    public void RegisterButton()
    {
        bool UN = false;
        bool EM = false;
        bool PW = false;
        bool CPW = false;

        if (Username != "") //checks to see if input field is empty
        {
            if (!System.IO.File.Exists(@"C:/UnityLogins/" + Username + ".txt")) //checks the file to see if the username entered exists
            {
                UN = true;
            }
            else
            {
                Debug.LogWarning("Username Taken");
            }
        }
        else
        {
            Debug.LogWarning("Username field Empty");
        }
        if(Email != "")
        {
            EmailValidation(); //calls email valid function
            if (EmailValid)
            {
                if (Email.Contains("@"))
                {
                    if (Email.Contains("."))
                    {
                        EM = true;
                    }
                    else
                    {
                        Debug.LogWarning("Email is Incorrect");
                    }
                }
                else
                {
                    Debug.LogWarning("Email is Incorrect");
                }
            }
            else
            {
                Debug.LogWarning("Email is Incorrect");
            }
        }
        else
        {
            Debug.LogWarning("Email Field Empty");
        }
        if(Password != "")
        {
            if(Password.Length > 5)
            {
                PW = true;
            }
            else
            {
                Debug.LogWarning("Password has to be 6 characters");
            }
        }
        else
        {
            Debug.LogWarning("Password Field Empty");
        }
        if(ConfPassword != "")
        {
            if(ConfPassword == Password)
            {
                CPW = true;
            }
            else
            {
                Debug.LogWarning("Passwords Don't Match");
            }

        }
        else
        {
            Debug.LogWarning("Confirm Password Field Empty");
        }
        if(UN == true && EM == true && CPW == true && PW == true)
        {
            bool Clear = true;
            int i = 1;
            foreach(char c in Password)
            {
                if (Clear)
                {
                    Password = "";
                    Clear = false;
                }
                i++;
                char Encrypted = (char)(c * i);
                Password += Encrypted.ToString();
            }
            form = (Username + "\n"+Email + "\n"+Password);
            System.IO.File.WriteAllText(@"C:/UnityLogins/" + Username + ".txt", form);
            username.text = "";
            email.text = "";
            password.text = "";
            confPassword.text = "";
            print("Registration Complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                email.Select();
            }
            if (email.isFocused)
            {
                password.Select();
            }
            if (password.isFocused)
            {
                confPassword.Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Password != "" && Email != "" && ConfPassword != "" && Username != "")
            {
                RegisterButton();
            }
        }
        Username = username.text;
        Email = email.text;
        Password = password.text;
        ConfPassword = confPassword.text;


    }
    void EmailValidation()
    {
        bool SW = false;
        bool EW = false;
        for(int i = 0; i<Characters.Length; i++)
        {
            if (Email.StartsWith(Characters[i]))
            {
                SW = true;
            }
        }
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Email.EndsWith(Characters[i]))
            {
                EW = true;
            }
        }
        if(SW == true && EW == true)
        {
            EmailValid = true;
        }
        else
        {
            EmailValid = false;
        }
    }
}
