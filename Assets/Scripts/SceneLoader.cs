using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioManager _audio;

    void Start()
    {
        _audio = FindObjectOfType<AudioManager>();
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Active Scene is now " + sceneName + ".");
        switch (sceneName)
        {
            case "MainMenu":
                _audio.PlaySong(0);
                break;
            case "Level01":
                _audio.PlaySong(1);
                break;
            default:
                _audio.PlaySong(9);
                break;
        }

        SceneManager.LoadScene(sceneName);
    }
}
