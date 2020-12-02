using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackPiece : MonoBehaviour
{
    public Vector2 dimensions = new Vector2(1, 1);
    public List<Transform> traversalPoints = new List<Transform>();
    [SerializeField]
    LayerMask ground, track;
    Vector3 mousePos;

    // Position and rotation for track to move to
    Vector3 movPoint;
    Quaternion rotPoint;

    // Used if piece is dropped in invalid place
    bool canBePlaced;
    Vector3 posBeforePickup;
    Quaternion rotBeforePickup;

    [SerializeField]
    Transform obj;

    [SerializeField]
    GameObject spaceIndicator;

    // Shows a misconnection if track is connected to station
    public bool notConnected;
    [SerializeField]
    GameObject notConnectedVisual;

    public float rotSpeed = 10f;
    public float movSpeed = 20f;

    [SerializeField]
    bool isStatic=false;

    public enum TrackState { NotPlaced, PickedUp, Placed };
    public TrackState state;

    public TrackConnector connector1, connector2;

    private void Awake()
    {
        movPoint = transform.position;
        rotPoint = transform.rotation;
        if (Mathf.Abs(rotPoint.eulerAngles.y) == 90 || Mathf.Abs(rotPoint.eulerAngles.y) == 270)
        {
            connector1.xOrYConnect *= -1;
            connector2.xOrYConnect *= -1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        traversalPoints.Clear();
        foreach(Transform point in transform.Find("TraversalPoints"))
        {
            traversalPoints.Add(point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, ground))
        {
            mousePos = hit.point;
        }
        else
        {
            canBePlaced = false;
        }

        switch (state)
        {
            case TrackState.NotPlaced:
                break;
            case TrackState.PickedUp:
                notConnectedVisual.SetActive(false);

                movPoint = new Vector3(Mathf.Round(mousePos.x*2f)*0.5f, 0f, Mathf.Round(mousePos.z*2f)*0.5f);
                obj.localPosition = new Vector3(0f, 0.25f, 0f);
                if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
                {
                    //Rotate
                    Vector3 rot = rotPoint.eulerAngles;
                    rot.y += 90;
                    rotPoint = Quaternion.Euler(rot);
                    connector1.xOrYConnect *= -1;
                    connector2.xOrYConnect *= -1;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, rotPoint, rotSpeed * Time.deltaTime);

                canBePlaced = Physics.OverlapBox(transform.position, new Vector3(dimensions.x, mousePos.y, dimensions.y), Quaternion.identity, track).Length <= 14;

                if (!Input.GetMouseButton(0))
                {
                    Drop();
                }
                break;
            case TrackState.Placed:
                if (notConnected)
                {
                    if (connector1.otherConnector == null)
                    {
                        notConnectedVisual.SetActive(true);
                        notConnectedVisual.transform.position = connector1.transform.position;
                    }
                    if (connector2.otherConnector == null)
                    {
                        notConnectedVisual.SetActive(true);
                        notConnectedVisual.transform.position = connector2.transform.position;
                    }
                }
                else
                {
                    notConnectedVisual.SetActive(false);
                }

                break;
            default:
                break;
        }

        transform.position = Vector3.Slerp(transform.position, movPoint, movSpeed * Time.deltaTime);
    }

    void PickUp()
    {
        state = TrackState.PickedUp;
        spaceIndicator.SetActive(true);
        posBeforePickup = transform.position;
        rotBeforePickup = transform.rotation;
    }

    void Drop()
    {
        // Snap to grid
        Vector3 placePosition = movPoint;
        placePosition.x = Mathf.Round(placePosition.x*2f)*0.5f;
        placePosition.z = Mathf.Round(placePosition.z*2f)*0.5f;
        placePosition.y = 0;

        if (connector1.canBeConnected && connector2.canBeConnected)
        {
            state = TrackState.Placed;
        }
        else
        {
            state = TrackState.NotPlaced;
        }

        transform.rotation = rotPoint;
        if (canBePlaced)
        {
            movPoint = placePosition;
            connector1.Connect();
            connector2.Connect();
        }
        else
        {
            movPoint = posBeforePickup;
            transform.rotation = rotBeforePickup;
            rotPoint = rotBeforePickup;
        }
        obj.localPosition = new Vector3(0f, 0.1f, 0f);

        spaceIndicator.SetActive(false);

        FindObjectOfType<CircuitLoop>().CheckLoop();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUp();
        }
    }
}
