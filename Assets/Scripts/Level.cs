using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject startButton, finishedScreen;
    PassengerController[] passengers;
    bool levelFinished;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        passengers = FindObjectsOfType<PassengerController>();
        startButton.SetActive(false);
        finishedScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelFinished)
        {
            if (passengers.Length > 0)
            {
                bool finished = true;

                //Passenger-controlled win
                foreach(PassengerController pc in passengers)
                {
                    if (pc.atCorrectStation) continue;
                    else { finished = false; break; }
                }
                if (finished)
                {
                    LevelComplete();
                }
            }
        }
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuIE());
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelIE());
    }

    public void Restart()
    {
        StartCoroutine(RestartIE());
    }

    public void ShowStartButton()
    {
        startButton.SetActive(true);
    }

    public void HideStartButton()
    {
        startButton.SetActive(false);
    }

    IEnumerator NextLevelIE()
    {
        anim.SetTrigger("Leave");
        yield return new WaitForSeconds(1f);
        Debug.Log("Load next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    IEnumerator MainMenuIE()
    {
        anim.SetTrigger("Leave");
        yield return new WaitForSeconds(1f);
        Debug.Log("MainMenu");
        SceneManager.LoadScene(0);
    }

    IEnumerator RestartIE()
    {
        anim.SetTrigger("Leave");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete()
    {
        levelFinished = true;
        finishedScreen.SetActive(true);
        FindObjectOfType<Timer>().StopTimer();
    }
}
