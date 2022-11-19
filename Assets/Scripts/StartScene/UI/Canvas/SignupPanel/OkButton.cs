using UnityEngine;
using UnityEngine.UI;

namespace StartScene.UI.Canvas.SignupPanel
{
    public class OkButton: MonoBehaviour
    {
        private Button _okButton;

        public GameObject mask;

        public GameObject password;

        public GameObject account;

        public GameObject confirmPassword;

        private void Start()
        {
            _okButton = transform.GetComponent<Button>();
            _okButton.onClick.AddListener(Ok);
        }

        private void Ok()
        {
            var acc = account.GetComponent<Text>().text;
            var psw = password.GetComponent<Text>().text;
            var cfmPsw = confirmPassword.GetComponent<Text>().text;
            if (!psw.Equals(cfmPsw))
            {
                //TODO:两次密码输入不一致
            }
            //TODO:发送请求
            Debug.Log($"注册账号{acc}, 密码{psw}");
            mask.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}