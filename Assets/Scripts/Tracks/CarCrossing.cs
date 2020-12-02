using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class CarCrossing : MonoBehaviour
{
    [SerializeField]
    private GameObject checkPoint;

    [SerializeField]
    [Tooltip("The name of the animation")]
    private string barrierRise, barrierDown;

    public bool carPassing = false;
    private Animation animation;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SwitchState(bool input) // call in playmode with the Correct Tag
    {
        

        if (carPassing != input)
        {
            carPassing = input;

            // set the correct connectionPoint

            MoveBarrier();

            checkPoint.SetActive(carPassing);

        }


    }


    void MoveBarrier()
    {
        animation.Blend(carPassing ? barrierRise : barrierDown);

    }
}
