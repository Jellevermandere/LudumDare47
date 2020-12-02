using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Station : MonoBehaviour
{
    public TrackPiece stationTrack;
    public Transform[] passengers = new Transform[0];
    public PassengerType type;
    [SerializeField] int capacity;
    [SerializeField] GameObject stoppers;
    [SerializeField] Transform stationPlatform;

    public Carriage train;
    [SerializeField] bool enabled;
    bool emptyingCarriage, carriageEmptied;

    public bool startingStation;

    [Header("Red Passengers")]
    public GameObject redPassenger;
    [SerializeField] int redNum;

    [Header("Blue Passengers")]
    public GameObject bluePassenger;
    [SerializeField] int blueNum;

    [Header("Green Passengers")]
    public GameObject greenPassenger;
    [SerializeField] int greenNum;

    private void Awake()
    {
        if (passengers.Length < capacity)
        {
            Transform[] temp = passengers;
            passengers = new Transform[capacity];
            for (int n = 0; n < temp.Length; n++)
            {
                if (temp[n] != null) passengers[n] = temp[n];
            }
        }

        if (redNum + blueNum + greenNum <= capacity)
        {
            for (int r = 0; r < redNum; r++)
            {
                passengers[r] = (GameObject.Instantiate(redPassenger)).transform;
            }
            for (int b = 0; b < blueNum; b++)
            {
                passengers[redNum + b] = (GameObject.Instantiate(bluePassenger)).transform;
            }
            for (int g = 0; g < greenNum; g++)
            {
                passengers[redNum + blueNum + g] = (GameObject.Instantiate(greenPassenger)).transform;
            }
        }

        for (int i = 0; i < capacity; i++)
        {
            float xVal = (2.4f / capacity) * i;
            if (passengers[i] != null) passengers[i].position = stationPlatform.position + new Vector3(xVal, 0f, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Collider>().enabled = enabled;

        if (enabled && train != null && !carriageEmptied && !emptyingCarriage)
        {
            StartCoroutine(this.LoadPassengers());
        }

        for(int i = 0; i < capacity; i++)
        {
            if (passengers[i] == null) return;
            float xVal = (2.4f / capacity) * i;
            passengers[i].position = Vector3.MoveTowards(passengers[i].position, stationPlatform.position + new Vector3(xVal, 0f, 0f), 3f*Time.deltaTime);
            passengers[i].localScale = Vector3.MoveTowards(passengers[i].localScale, new Vector3(1, 1, 1), 3f*Time.deltaTime);
        }
    }

    public bool AddPassenger(PassengerController pc)
    {
        for(int i = 0; i < capacity; i++)
        {
            if (passengers[i] == null)
            {
                passengers[i] = pc.transform;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carriage"))
        {
            Carriage t = other.GetComponent<Carriage>();
            if (t != null)
            {
                train = t;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Carriage t = other.GetComponent<Carriage>();
        if (t != null)
        {
            StartCoroutine(LeavingStation());
        }
    }

    IEnumerator LoadPassengers()
    {
        Debug.Log(gameObject.name);
        stoppers.SetActive(true);

        emptyingCarriage = true;
        carriageEmptied = false;

        Train mainTrain = train.carriages[0];
        while (mainTrain._trainSpeed > 0)
        {
            yield return null;
        }
        if (train == null)
        {
            stoppers.SetActive(false);
            emptyingCarriage = false;
            carriageEmptied = false;
            yield break;
        }

        yield return StartCoroutine(train.LoadPassengers(this));

        Debug.Log("passengers unloaded");

        for(int i = 0; i < passengers.Length; i++)
        {
            if (passengers[i] != null && !passengers[i].GetComponent<PassengerController>().atCorrectStation)
            {
                if (train.AddPassenger(passengers[i].GetComponent<PassengerController>()))
                {
                    //Load Passenger into carriage
                    passengers[i] = null;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("passengers loaded");

        yield return new WaitForSeconds(1f);

        Debug.Log("Go again");

        emptyingCarriage = false;
        carriageEmptied = true;

        stoppers.SetActive(false);
    }

    IEnumerator LeavingStation()
    {
        yield return new WaitForSeconds(1f);
        train = null;
        carriageEmptied = false;
    }

    public void EnableStation()
    {
        enabled = true;
    }
}
