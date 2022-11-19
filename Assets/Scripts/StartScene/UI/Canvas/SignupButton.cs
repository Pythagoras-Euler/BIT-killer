using UnityEngine;
using UnityEngine.UI;

namespace StartScene.UI.Canvas
{
    public class SignupButton : MonoBehaviour
    {
        private Button _login;

        public GameObject signupPanel;

        public GameObject mask;

        void Start()
        {
            _login = transform.GetComponent<Button>();
            _login.onClick.AddListener(LogIn);
        }

        private void LogIn()
        {
            signupPanel.SetActive(true);
            mask.SetActive(true);
        }
    }
}
