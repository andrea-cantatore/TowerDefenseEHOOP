using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenù : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
