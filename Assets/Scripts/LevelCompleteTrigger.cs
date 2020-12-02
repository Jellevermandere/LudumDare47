using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            Train t = other.GetComponent<Train>();
            if (t != null && t.mainTrain)
            {
                FindObjectOfType<Level>().LevelComplete();
                activated = true;
            }
        }

    }
}
