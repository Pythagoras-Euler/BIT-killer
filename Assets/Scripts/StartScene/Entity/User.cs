using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string type { get; set; }
    public Dictionary<string, string> content;
    public User(string t, string u, string p)
    {
        type = t;
        content = new Dictionary<string, string>()
                {
                    { "username",u},
                    { "password",p }
                };
    }

}
public class RetUser
{
    public string type { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public Dictionary<string, string> content;

    public RetUser() { }
   /* public RetUser(string t, bool s, string m)
    {
        type = t;
        success = s;
        message = m;
        content = new Dictionary<string, string>()
        {
        };
    }*/
}

