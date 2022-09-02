using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("OptionsScreen");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application has exited.");
    }
}
