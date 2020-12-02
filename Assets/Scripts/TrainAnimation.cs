using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Train), typeof(AudioSource))]
public class TrainAnimation : MonoBehaviour
{

    [SerializeField]
    private Transform rearWheelBolt, piston, shaftGoal, rearWheel, body;

    [SerializeField]
    private AudioSource trainNoise;

    [SerializeField]
    ParticleSystem smokeFX;

    public bool moveSelf = false;

    [Header("Parameters")]
    [SerializeField]
    private float wheelSpeedMultiplier;
    [SerializeField]
    private float maxDeviation = 0.02f;
    [SerializeField]
    private float vibrationFrames;

    private Train trainController;
    private Vector3 startBodyPos;

    // Start is called before the first frame update
    void Start()
    {
        trainController = GetComponent<Train>();
        trainNoise = GetComponent<AudioSource>();

        startBodyPos = body.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (trainController.moving || moveSelf)
        {
            rearWheel.Rotate(Vector3.left * trainController._trainSpeed * Time.deltaTime * wheelSpeedMultiplier);

            AnimateTrain();

            body.localPosition = startBodyPos + new Vector3(Random.Range(-maxDeviation, maxDeviation), Random.Range(-maxDeviation, maxDeviation), Random.Range(-maxDeviation, maxDeviation))*(trainController._trainSpeed > 0 ? trainController._trainSpeed/trainController.trainSpeed : 0);

            if(!trainNoise.isPlaying) trainNoise.Play();

        }
        else
        {
            trainNoise.Stop();
        }

        //smokeFX.enableEmission = trainController._trainSpeed > 0;


    }



    void AnimateTrain()
    {
        // Piston animations

        piston.position = rearWheelBolt.position;

        piston.forward = rearWheelBolt.position - shaftGoal.position;

        // Smoke particle fx

    }
}
