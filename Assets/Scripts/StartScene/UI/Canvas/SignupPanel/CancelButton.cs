using UnityEngine;
using UnityEngine.UI;

namespace StartScene.UI.Canvas.SignupPanel
{
    public class CancelButton: MonoBehaviour
    {
        private Button _cancelButton;

        public GameObject mask;

        private void Start()
        {
            _cancelButton = transform.GetComponent<Button>();
            _cancelButton.onClick.AddListener(Cancel);
        }

        private void Cancel()
        {
            mask.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}