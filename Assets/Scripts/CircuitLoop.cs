using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CircuitLoop : MonoBehaviour
{
    Station startingStation;
    List<Transform> traversalPoints = new List<Transform>();
    List<TrackPiece> traversalTracks = new List<TrackPiece>();
    int currentTrainPosInd=1;
    bool looped = false;
    [SerializeField] bool checkAgain = false;

    public Train train;
    public float trainSpeed, trainRotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startingStation = GetComponent<Station>();
    }

    bool Traverse(TrackPiece currentPiece, TrackPiece lastPiece, List<TrackPiece> traversedPieces)
    {
        if (currentPiece == null) currentPiece = startingStation.stationTrack;

        if (traversedPieces == null)
        {
            traversedPieces = new List<TrackPiece>() { currentPiece };
            traversalTracks.Clear();
        }
        else
        {
            traversedPieces.Add(currentPiece);
        }

        currentPiece.notConnected = false;

        if (lastPiece == null)
        {
            if (currentPiece.connector1.otherConnector != null)
            {
                traversalPoints.Add(currentPiece.traversalPoints[0]);
                return Traverse(currentPiece.connector1.otherConnector.piece, currentPiece, traversedPieces);
            }
            else return false;
        }

        if (currentPiece.connector1.otherConnector != null && !Contains(traversedPieces, currentPiece.connector1.otherConnector.piece) && currentPiece.connector1.otherConnector.piece != lastPiece)
        {
            for(int i = currentPiece.traversalPoints.Count-1; i >= 0; i--)
            {
                traversalPoints.Add(currentPiece.traversalPoints[i]);
            }
            traversalTracks.Add(currentPiece);
            return Traverse(currentPiece.connector1.otherConnector.piece, currentPiece, traversedPieces);
        }

        else if (currentPiece.connector2.otherConnector != null && !Contains(traversedPieces, currentPiece.connector2.otherConnector.piece) && currentPiece.connector2.otherConnector.piece != lastPiece)
        {
            for (int i = 0; i < currentPiece.traversalPoints.Count; i++)
            {
                traversalPoints.Add(currentPiece.traversalPoints[i]);
            }
            traversalTracks.Add(currentPiece);
            return Traverse(currentPiece.connector2.otherConnector.piece, currentPiece, traversedPieces);
        }

        else
        {
            if (currentPiece == startingStation.stationTrack)
            {
                traversalTracks.Add(currentPiece);
                return true;
            }
            else
            {
                currentPiece.notConnected = true;
                for (int i = 0; i < currentPiece.traversalPoints.Count; i++)
                {
                    traversalPoints.Add(currentPiece.traversalPoints[i]);
                }
                traversalTracks.Add(currentPiece);
                return false;
            }
        }
    }

    void Traverse2()
    {
        TrackPiece currentPiece = traversalTracks.Count == 0 ? startingStation.stationTrack : traversalTracks[traversalTracks.Count - 1];

        TrackConnector con1 = currentPiece.connector1.otherConnector;
        TrackConnector con2 = currentPiece.connector2.otherConnector;

        TrackPiece nextPiece = null;

        if (con1 != null && (traversalTracks.Count > 1 ? con1.piece != traversalTracks[traversalTracks.Count-2] : true) && !Contains(traversalTracks, con1.piece))
        {
            nextPiece = con1.piece;

            for (int i = currentPiece.traversalPoints.Count - 1; i >= 0; i--)
            {
                if (currentPiece.traversalPoints[i].gameObject.active) traversalPoints.Add(currentPiece.traversalPoints[i]);
            }
        }

        else if (con2 != null && (traversalTracks.Count > 1 ? con2.piece != traversalTracks[traversalTracks.Count - 2] : true) && !Contains(traversalTracks, con2.piece))
        {
            nextPiece = con2.piece;

            for (int i = 0; i < currentPiece.traversalPoints.Count; i++)
            {
                if (currentPiece.traversalPoints[i].gameObject.active) traversalPoints.Add(currentPiece.traversalPoints[i]);
            }
        }

        if (nextPiece == null)
        {
            if (con1 != null && con2 != null && (con1.piece == traversalTracks[0] || con2.piece == traversalTracks[0]))
            {
                looped = true;
            }
            return;
        }

        if (traversalTracks.Count == 0) traversalTracks.Add(currentPiece);
        traversalTracks.Add(nextPiece);
        Traverse2();
        if (looped) return;
        else
        {
            //traversalTracks.Remove(nextPiece);
        }
    }

    bool Contains(List<TrackPiece> pieces, TrackPiece piece)
    {
        foreach(TrackPiece tp in pieces)
        {
            if (tp == piece) return true;
        }
        return false;
    }

    public void CheckLoop()
    {
        traversalPoints = new List<Transform>();
        traversalTracks = new List<TrackPiece>();
        //looped = Traverse(null, null, null);
        Traverse2();
        if (looped)
        {
            FindObjectOfType<Level>().ShowStartButton();
        }
        else
        {
            FindObjectOfType<Level>().HideStartButton();
        }
    }

    public void StartTrain()
    {
        foreach(TrackPiece tp in FindObjectsOfType<TrackPiece>())
        {
            tp.enabled = false;
        }
        train.SetPositions(traversalPoints);
        FindObjectOfType<Level>().HideStartButton();
        StartCoroutine(WaitToEnableStations());
        FindObjectOfType<Timer>().StartTimer();
    }

    IEnumerator WaitToEnableStations()
    {
        foreach (Station s in FindObjectsOfType<Station>())
        {
            if (s.startingStation) s.EnableStation();
        }

        yield return new WaitForSeconds(3f);

        foreach (Station s in FindObjectsOfType<Station>())
        {
            s.EnableStation();
        }
    }

    private void Update()
    {
        if (checkAgain)
        {
            CheckLoop();
            checkAgain = false;
        }
    }
}
