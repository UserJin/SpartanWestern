using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void PressStartBtn()
    {
        //SceneManager.LoadScene("tutorial");
        SceneManager.LoadScene(1);
    }

    public void PressQuitBtn()
    {
        Application.Quit();
    }
}
