using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("My Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
