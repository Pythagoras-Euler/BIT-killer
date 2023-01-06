using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBtn : MonoBehaviour
{
    public int scene;
    public void changeScene()
    {
        SceneManager.LoadScene(scene);
    }
}
