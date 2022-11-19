using UnityEngine;
using UnityEngine.UI;

namespace StartScene.UI.Canvas
{
    public class LoginButton : MonoBehaviour
    {
        private Button _login;

        public GameObject loginPanel;

        public GameObject mask;

        void Start()
        {
            _login = transform.GetComponent<Button>();
            _login.onClick.AddListener(LogIn);
        }
    
        private void LogIn()
        {
            loginPanel.SetActive(true);
            mask.SetActive(true);
        }

    }
}
