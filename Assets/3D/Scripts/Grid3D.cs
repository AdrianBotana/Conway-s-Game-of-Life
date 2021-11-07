using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3D : MonoBehaviour
{
    [SerializeField] float gridSize;
    [SerializeField] float cellSize;

    public static float GridSize { get { return instance.gridSize; } }
    public static float CellSize { get { return instance.cellSize; } }

    private static Grid3D instance;

    private void OnValidate()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
    }

    private void OnDestroy()
    {
        if (instance = this)
            instance = null;
    }
}
