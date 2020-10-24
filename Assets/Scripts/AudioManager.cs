using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    [SerializeField] AudioClip _startingSong = null;
    [SerializeField] AudioClip _combatSong = null;

    AudioSource _audioSource;

    string currentScene;

    private void Awake()
    {
        //currentScene = SceneManager.GetActiveScene().name;
        //Debug.Log("Active Scene is " + currentScene + ".");
        #region Singleton Pattern (Simple)
        if(Instance == null)
        {
            //doesn't exist yet, this is now our simpleton singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //fill references
            _audioSource = GetComponent<AudioSource>();
            //PlaySong(_startingSong);
            PlaySong(0);
        }
        else
        {
            //or else you will DIE!
            Destroy(gameObject);
        }
        #endregion
    }
    /*
    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    */
    public void PlaySong(int songNumber)
    {
        switch (songNumber)
        {
            case 0:
                Debug.Log("Playing Exploration music...");
                _audioSource.Stop();
                _audioSource.PlayOneShot(_startingSong, .3f);
                break;
            case 1:
                Debug.Log("Playing Combat music...");
                _audioSource.Stop();
                _audioSource.PlayOneShot(_combatSong, .3f);
                break;

            default:
                Debug.Log("Stopping Music");
                _audioSource.Stop();
                break;
        }
    }
}
