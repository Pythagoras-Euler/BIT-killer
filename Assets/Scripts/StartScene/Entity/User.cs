using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User:MonoBehaviour
{
    public string type { get; set; }
    public Dictionary<string, string> content;
    public User(string t, string u, string p, string st)
    {
        type = t;
        content = new Dictionary<string, string>()
                {
                    { "username",u},
                    { "password",p },
                    { "salt", st }
                };
    }

}
public class RetUser
{
    public string type { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public string salt { get; set; }

    public Dictionary<string, string> content;

    public RetUser() { }
    public RetUser(string t, bool s, string m)
    {
        type = t;
        success = s;
        message = m;
        //salt = st;
        content = new Dictionary<string, string>()
        {
        };
    }
}

