using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    public bool isOld;


    private void Awake()
    {
        
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetOld()
    {
        isOld = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0 && isOld)
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
        else isOld = true;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) isOld = true;
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}

