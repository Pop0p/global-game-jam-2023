using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnPlay()
    {
        // aller dans une scène
        SceneManager.LoadScene("Jeu");
    }

    public void OnExit()
    {
        // exit app
        Application.Quit();
    }
}
