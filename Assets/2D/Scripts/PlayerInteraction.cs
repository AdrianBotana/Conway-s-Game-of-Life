using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GridSimulation gridSimulation;
    [SerializeField] float stepDelay;

    float elapsedTime;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.transform.TryGetComponent(out GridPoint point) )
                point.SwichState();
        }

        if (Input.GetKey(KeyCode.Space) && elapsedTime > stepDelay)
        {
            elapsedTime = 0;
            gridSimulation.SimulationStep();
        }

        elapsedTime += Time.deltaTime;
    }
}
