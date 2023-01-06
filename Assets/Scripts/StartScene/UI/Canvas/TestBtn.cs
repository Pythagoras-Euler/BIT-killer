using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestBtn : MonoBehaviour
{
    public int scene;
    public void changeScene()
    {
        Debug.Log("CLICK");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
