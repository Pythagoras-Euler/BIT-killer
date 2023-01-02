using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class LoginCfmBtn : MonoBehaviour
{
    public GameObject mask;
    public Text account;
    public Text password;
    public Text promptInfo;

    // Start is called before the first frame update

    string sha256(string plaintext)
    {
        string cipertext;
        cipertext = plaintext;//TODO �������

        return cipertext;
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

            string cipcher = sha256(psw);

            //TODO:��������
            Debug.Log($"��ʶ\"login\",�˺�{acc}, ����{cipcher}");
            //


            string retType = "login";//TODO ���շ���������ֵ
            bool retSuccess = true;
            string retMes = "Login successful";


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
                                                                                     //����Ҫ�� �ļ�Fiile->
                                                                                     //��������BuildSettings ������˳��
                                                                                     //�����Ҫ�Ļ���������һ�����ɶ���

                //loginAccount.text = acc;
                //loginPassword.text = psw;

            }
        }


    }
    
}
