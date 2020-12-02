using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Transform winder;

    [SerializeField]
    private float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateWinder();
    }

    void RotateWinder()
    {
        winder.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
