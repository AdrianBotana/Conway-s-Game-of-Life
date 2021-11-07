using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction3D : MonoBehaviour
{
    [SerializeField] BaseGridSimulation gridSimulation;
    [SerializeField] float stepDelay;
    [SerializeField] bool autoPlay;

    float elapsedTime;

    void Update()
    {
        if ((autoPlay || Input.GetKey(KeyCode.Space)) && elapsedTime > stepDelay)
        {
            elapsedTime = 0;
            gridSimulation.SimulationStep();
        }

        elapsedTime += Time.deltaTime;
    }
}
