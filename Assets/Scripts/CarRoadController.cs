using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EdgeCollider2D))]
public class CarRoadController : MonoBehaviour
{

    private EdgeCollider2D edgeCol;

    [SerializeField]
    private GameObject car;

    [SerializeField]
    private string carCrossingTag = "CrossRoad";
    [SerializeField]
    private int nrOfCars = 10;
    [SerializeField]
    private int minDistBTWCars = 3;

    [SerializeField]
    private float carSpeed = 10f;

    public bool driving;


    private GameObject[] carCrossings;
    private List<GameObject> cars = new List<GameObject>();
    private List<int> carPositions = new List<int>();
    private float progress;

    

    // Start is called before the first frame update
    void Start()
    {
        edgeCol = GetComponent<EdgeCollider2D>();

        carCrossings = GameObject.FindGameObjectsWithTag(carCrossingTag);

        SpawnCars(nrOfCars);

    }

    // Update is called once per frame
    void Update()
    {

        if (driving)
        {
            progress += carSpeed * Time.deltaTime;

            if(progress > 1)
            {
                progress = 0;

                for (int i = 0; i < carPositions.Count; i++)
                {
                    carPositions[i] = (carPositions[i] + 1) % edgeCol.pointCount;
                }
            }

            UpdatePositions();

        }

    }
    // spawn the cars certain distances away from eachother
    void SpawnCars(int nr)
    {
        int totalPoints = edgeCol.pointCount;

        for (int i = 0; i < nr; i++)
        {
            GameObject newCar = Instantiate(car, transform);
            carPositions.Add(i * minDistBTWCars % totalPoints);
            cars.Add(newCar);

            newCar.transform.position = FlatToWorld(edgeCol.points[i * minDistBTWCars % totalPoints]);
            newCar.transform.LookAt(FlatToWorld(edgeCol.points[(i * minDistBTWCars + 1) % totalPoints]));
        }
    }

    // update the cars position
    void UpdatePositions()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            Vector3 nextPos = FlatToWorld(edgeCol.points[(carPositions[i] + 1) % edgeCol.pointCount]);

            cars[i].transform.LookAt(Vector3.Lerp(FlatToWorld(edgeCol.points[(carPositions[i] + 1) % edgeCol.pointCount]), FlatToWorld(edgeCol.points[(carPositions[i] + 2) % edgeCol.pointCount]), progress));
            cars[i].transform.position = Vector3.Lerp(FlatToWorld(edgeCol.points[carPositions[i]]), nextPos, progress);
            
        }


    }


    // helper function to translate the edgecollider to flat world
    Vector3 FlatToWorld(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }
}
