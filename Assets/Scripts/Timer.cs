using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject timerText;
    float currentTime;
    bool counting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            currentTime += Time.deltaTime;
        }

        timerText.GetComponent<Text>().text = currentTime.ToString("#0.000");
    }

    public void StartTimer()
    {
        timerText.SetActive(true);
        counting = true;
    }

    public void StopTimer()
    {
        counting = false;
    }
}
