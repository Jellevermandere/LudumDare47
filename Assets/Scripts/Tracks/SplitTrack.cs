using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animation))]
public class SplitTrack : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The name of the animation")]
    private string arrowForward, arrowBack;

    private bool atStart = true;
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

    public void SwitchSides() // call in playmode with the Correct Tag
    {
        // set the correct connectionPoint

        //switch the arrow

        MoveArrow();

    }

    void MoveArrow()
    {
        animation.Blend(atStart? arrowForward : arrowBack);
        
        atStart = !atStart;
    }


}
