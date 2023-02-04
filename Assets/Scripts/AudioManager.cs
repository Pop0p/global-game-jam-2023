using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioSource[] AudioSources;
    public AudioClip[] Musics;
    public AudioClip[] Voices;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSound(string moment)
    {
        switch (moment)
        {
            case "flower 1":
                AudioSources[0].clip = Voices[0];
                break;
            case "flower 2":
                AudioSources[0].clip = Voices[2];
                break;
            case "flower 3":
                AudioSources[0].clip = Voices[3];
                break;
            case "flower 5":
                AudioSources[0].clip = Voices[4];
                break;
            case "victory": // victory
                AudioSources[1].clip = Musics[1];
                break;
            case "death":
                AudioSources[1].clip = Musics[2];
                break;
            case "begin": // après lecture du tuto
                AudioSources[1].clip = Musics[0];
                break;
            default:
                break;
        }
    }
}
