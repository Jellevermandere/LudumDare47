using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    List<Transform> traversalPoints;
    int ind=7;
    public bool moving, mainTrain;
    public float trainSpeed, trainRotSpeed;
    public float _trainSpeed;
    public List<Train> carriages = new List<Train>();

    public TrackPiece currentTrack;

    [SerializeField]
    LayerMask obstacle;
    [SerializeField]
    Transform checkPos;

    protected void Update()
    {
        if (moving)
        {
            Vector3 currentPos = transform.position;
            Vector3 nextPos = traversalPoints[ind + 1].position;

            if (!mainTrain) // Train is a carriage, main train is index 0
            {
                transform.position = Vector3.MoveTowards(currentPos, nextPos, carriages[0]._trainSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-currentPos + nextPos), carriages[0].trainRotSpeed * Time.deltaTime);
            }

            else // Train is the main engine
            {
                transform.position = Vector3.MoveTowards(currentPos, nextPos, _trainSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-currentPos + nextPos), trainRotSpeed * Time.deltaTime);

                Collider[] c = Physics.OverlapSphere(checkPos.position, 0.2f, obstacle);
                if (c.Length > 0) // Switch encountered
                {
                    if (_trainSpeed > 0)
                    {
                        _trainSpeed -= 2 * Time.deltaTime;
                    }
                    else _trainSpeed = 0f;
                }
                else
                {
                    if (_trainSpeed < trainSpeed) _trainSpeed += Time.deltaTime;
                    else _trainSpeed = trainSpeed;
                }
            }

            if (Vector3.Distance(currentPos, nextPos) < 0.1f)
            {
                if (++ind >= traversalPoints.Count - 1) ind = 0;
            }
        }
    }

    public void SetPositions(List<Transform> travPoints)
    {
        moving = true;
        traversalPoints = travPoints;
        if (carriages.Count == 2)
        {
            foreach (Train carriage in carriages)
            {
                carriage.SetPositions(travPoints);
            }
            if (ind < 0)
            {
                ind = traversalPoints.Count - ind;
            }
        }
    }
}
