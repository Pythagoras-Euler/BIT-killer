using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPanelScript : MonoBehaviour
{
    public GameObject exitPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ExitPanelAct()
    {
        exitPanel.SetActive(true);
    }

    public void Back2Start()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        exitPanel.SetActive(false);
    }

}
