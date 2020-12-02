using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackConnector : MonoBehaviour
{
    [SerializeField]
    bool enabled = true; // Used on station tracks so they don't decouple from other station tracks

    public bool canBeConnected;
    public int xOrYConnect; // x = 1, y = -1
    public TrackConnector otherConnector = null;
    public TrackPiece piece;

    [SerializeField]
    MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend.enabled = false;
        piece = transform.parent.GetComponentInParent<TrackPiece>();
        if (enabled) Connect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enabled && other.CompareTag("Connector") && otherConnector == null && other.GetComponent<TrackConnector>().otherConnector == null && xOrYConnect == other.GetComponent<TrackConnector>().xOrYConnect)
        {
            canBeConnected = true;
            otherConnector = other.GetComponent<TrackConnector>();
            rend.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enabled && otherConnector != null && other.CompareTag("Connector") && other.GetComponent<TrackConnector>() == otherConnector && (piece.state == TrackPiece.TrackState.PickedUp || other.GetComponent<TrackConnector>().piece.state == TrackPiece.TrackState.PickedUp))
        {
            //Debug.Log(piece.gameObject.name + " disconnected from " + otherConnector.piece.gameObject.name);
            otherConnector = null;
            rend.enabled = false;
        }
    }

    public void Connect(bool isOtherConnected=false, TrackConnector other=null)
    {
        if (!enabled) return;

        // Get all nearby connectors
        if (!isOtherConnected)
        {
            Collider[] nearbyConnections = Physics.OverlapSphere(transform.position, 0.5f);
            foreach (Collider c in nearbyConnections)
            {
                if (c.CompareTag("Connector") && c.GetComponent<TrackConnector>().xOrYConnect == xOrYConnect && c != GetComponent<Collider>())
                {
                    otherConnector = c.GetComponent<TrackConnector>();
                    otherConnector.Connect(true, this);
                }
            }
        }
        else
        {
            otherConnector = other;
        }  

        rend.enabled = false;
    }
}
