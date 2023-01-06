using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{
    //public Text promptInfo;
    //public GameObject mask;

    public GameObject mask;
    public Text account;
    public Text password;
    public Text promptInfo;
    public WebLink wl;
    public InputField pswInputField;
    public Toggle pswDisplayTog;


    // Start is called before the first frame update



    void Start()
    {
        promptInfo.text = "";

        wl = GameObject.Find("WebLink").GetComponent<WebLink>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void BackToMainmenu()
    {
        gameObject.SetActive(false);
        mask.SetActive(false);
    }

    public void PswDisplay()
    {
        pswInputField.contentType = pswDisplayTog.isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
        pswInputField.Select();
    }


    // Start is called before the first frame update
    //private void Start()
    //{
    //    wl = GameObject.Find("WebLink").GetComponent<WebLink>();
    //}

    byte[] Sha256(string plaintext)
    {
        //string cipertext;
        //cipertext = plaintext;//TODO �������

        //From https://github.com/jv-amorim/Unity-Helpers/blob/master/Scripts/HashingHelpers/HashGenerator.cs with MIT Lisence
        byte[] data = Encoding.ASCII.GetBytes(plaintext);
        data = new SHA256Managed().ComputeHash(data);
        //string cipertext = Encoding.ASCII.GetString(data);   

        //return cipertext;
        return data;
    }


    public static string ToHexStrFromByte(byte[] byteDatas)//�����ã�log�������16�����ַ���
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < byteDatas.Length; i++)
        {
            builder.Append(string.Format("{0:X2} ", byteDatas[i]));
        }
        return builder.ToString().Trim();
    }
    public static string ToHexString(string plainString, Encoding encode)
    {
        byte[] byteDatas = encode.GetBytes(plainString);
        return ToHexStrFromByte(byteDatas);
    }

    public void LoginCfmBtnClicked()
    {

        var acc = account.GetComponent<Text>().text;
        var psw = password.GetComponent<Text>().text;
        if (psw.Length <= 6)
        {
            promptInfo.text = "���������6λ������";
        }
        else
        {

            //string cipcher = Sha256(psw);
            byte[] cipchers = Sha256(psw);
            string cipcher = Encoding.UTF8.GetString(cipchers);
            string cipchert = ToHexStrFromByte(cipchers);

            byte[] ax = new byte[2] { 0xFF, 0xF0 };

            //string opPsw = ToHexString(cipcher, Encoding.ASCII);
            Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipchers}");
            Debug.Log($"ff00:{ToHexStrFromByte(ax)}");
            Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipchert}");

            //TODO:��������
            User user = new User("login", acc, cipcher);
            string userJson = JsonUtility.ToJson(user);
            wl.Send(userJson);

            // ������ֵ
            RetUser retuser = JsonUtility.FromJson<RetUser>(wl.receiveJson);
            if (retuser.type == "login") // ����ǵ�¼ 
            {

                string retType = retuser.type;//���շ���������ֵ
                bool retSuccess = retuser.success;
                string retMes = retuser.message;


                if (retType != "login")//��֪��ʲôʱ���������ִ��󣨴�����û��ҵ㣿
                {
                    promptInfo.text = "δ֪����(����Ƶ��������";
                }
                else if (retSuccess == false)//���س��ִ���
                {
                    if (retMes == "Unvalid Username")//��½ʧ��
                    {
                        promptInfo.text = "���û���δ��ע��";//�û�������
                    }
                    else if (retMes == "Wrong Password")
                    {
                        promptInfo.text = "�������";//�������

                    }
                    else if (retMes == "Login Failed")//��½ʧ��
                    {
                        promptInfo.text = "��½ʧ�ܣ�������";//�ǲ���Ӧ�����û��������ں�����������ּ�鷽ʽ
                    }
                    else//��������
                    {
                        promptInfo.text = retMes;
                    }
                }
                else if (retMes == "Login successful")//�ɹ�
                {

                    promptInfo.text = "��¼�ɹ���";
                    mask.SetActive(false);
                    transform.parent.parent.gameObject.SetActive(false);
                    //TODO close Panel
                    //TODO set loginPanel true
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//�л�����һ���������˵����棨�����棩
                                                                                         //����Ҫ�� �ļ�File->
                                                                                         //��������BuildSettings ������˳��
                                                                                         //�����Ҫ�Ļ���������һ�����ɶ���


                }
            }
        }


    }

}
