using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Carriage : Train
{
    public int capacity;
    public List<Transform> passengersOnBoard = new List<Transform>();
    bool emptyingCarriage, carriageEmptied;
    [SerializeField] Transform passengerPoint;
    [SerializeField] TextMeshProUGUI capacityIndicatorR, capacityIndicatorB, capacityIndicatorG, capacityIndicatorAll1, capacityIndicatorAll2;
    [SerializeField] Color redCol, blueCol, greenCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        foreach(Transform t in passengersOnBoard)
        {
            if (_trainSpeed > 0) t.position = passengerPoint.position;
            else t.position = Vector3.MoveTowards(t.position, passengerPoint.position, 3f*Time.deltaTime);

            t.localScale = Vector3.MoveTowards(t.localScale, Vector3.zero, 3f*Time.deltaTime);
        }

        if (passengersOnBoard.Count > capacity) Debug.Log("TOO MANY PEOPLE");

        int r = 0;
        int b = 0;
        int g = 0;
        foreach(Transform t in passengersOnBoard)
        {
            if (t.GetComponent<PassengerController>().type == PassengerType.Red)
            {
                r++;
            }
            else if (t.GetComponent<PassengerController>().type == PassengerType.Blue)
            {
                b++;
            }
            else
            {
                g++;
            }
        }

        capacityIndicatorAll1.text = capacity.ToString();
        capacityIndicatorAll2.text = capacity.ToString();
        capacityIndicatorR.text = r.ToString();
        capacityIndicatorB.text = b.ToString();
        capacityIndicatorG.text = g.ToString();
        
        capacityIndicatorR.color = redCol;
        capacityIndicatorB.color = blueCol;
        capacityIndicatorG.color = greenCol;

        capacityIndicatorR.transform.rotation = Camera.main.transform.rotation;
        capacityIndicatorB.transform.rotation = Camera.main.transform.rotation;
        capacityIndicatorG.transform.rotation = Camera.main.transform.rotation;
    }

    public bool AddPassenger(PassengerController pc)
    {
        if (passengersOnBoard.Count >= capacity) return false;
        else
        {
            passengersOnBoard.Add(pc.transform);
            return true;
        }
    }

    public void RemovePassenger(PassengerController pc)
    {
        passengersOnBoard.Remove(pc.transform);
    }

    public IEnumerator LoadPassengers(Station station)
    {
        Transform[] temp = new Transform[passengersOnBoard.Count];
        for(int n=0; n < temp.Length; n++)
        {
            temp[n] = passengersOnBoard[n];
        }

        foreach(Transform t in temp)
        {
            PassengerController pc = t.GetComponent<PassengerController>();
            if (station.type == pc.type)
            {
                Debug.Log("Passenger is at right station.");
                //If right type
                if(station.AddPassenger(pc))
                {
                    //Unload passenger
                    pc.atCorrectStation = true;
                    RemovePassenger(pc);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
