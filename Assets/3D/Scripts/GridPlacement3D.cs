using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacement3D : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * Grid3D.GridSize);
    }
}
