using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class CrossingTrigger : MonoBehaviour
{

    [SerializeField]
    private CarCrossing carCrossing;
    [SerializeField]
    private LayerMask carMask;

    private void Update()
    {
        if( Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, carMask).Length > 0 && carCrossing)
        {
            Debug.Log("Inside");
            carCrossing.SwitchState(true);
        }
        else
        {
            carCrossing.SwitchState(false);
        }
    }

}
