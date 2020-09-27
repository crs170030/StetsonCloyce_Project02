using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    [SerializeField] AudioClip _startingSong;

    AudioSource _audioSource;

    private void Awake()
    {
        #region Singleton Pattern (Simple)
        if(Instance == null)
        {
            //doesn't exist yet, this is now our simpleton singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //fill references
            _audioSource = GetComponent<AudioSource>();
            //PlaySong(_startingSong);
            PlaySong();
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
    public void PlaySong()
    {
        _audioSource.PlayOneShot(_startingSong, .3f);
    }
}
