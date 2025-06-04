using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void MenuStart()
    {
        SceneManager.LoadScene("Environment");
    }
    public void exit()
    {
        Application.Quit();
    }
}
