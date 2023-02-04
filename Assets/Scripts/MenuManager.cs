using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource Audio;

    public void OnPlay()
    {
        Audio.Play();
        // aller dans une scène
        SceneManager.LoadScene("Jeu");
    }

    public void OnExit()
    {
        Audio.Play();
        // exit app
        Application.Quit();
    }
}
