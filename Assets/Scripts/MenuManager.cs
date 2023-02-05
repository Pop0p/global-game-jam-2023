using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource Audio;

    public void OnPlay()
    {
        if (Audio != null)
            Audio.Play();
        // aller dans une scène
        SceneManager.LoadScene("Jeu");
    }

    public void OnExit()
    {
        if (Audio != null)
            Audio.Play();
        // exit app
        Application.Quit();
    }
}
