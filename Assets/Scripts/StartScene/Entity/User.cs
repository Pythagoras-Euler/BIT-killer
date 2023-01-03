using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public string type;
    public string username;
    public string password;

    public User(string Type,string Username,string Password)
    {
        type = Type;
        username = Username;
        password = Password;

    }
    
}
public class RetUser
{
    public string type;
    public bool success;
    public string message;
}

