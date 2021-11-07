using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridComputerShaderSimulation3D : BaseGridSimulation
{
    [SerializeField] GridDrawer3D drawer;
    [SerializeField] ComputeShader computeShader;
    [Space]
    [SerializeField] float simulationCubeSize;
    [SerializeField] float cellSize;
    [Header("Simuation Parameters")]
    [SerializeField] int minNeighboursToSurvive = 2;
    [SerializeField] int maxNeighboursToSurvive = 3;
    [SerializeField] int minNeighboursToRevive = 3;
    [SerializeField] int maxNeighboursToRevive = 3;

    private bool[,,] gridItems;

    private List<Vector4> activeCells = new List<Vector4>();

    private int cellNumber;
    private Vector3 origin;

    private void Awake()
    {
        cellNumber = (int)(simulationCubeSize / cellSize);
        origin = transform.position - Vector3.one * (simulationCubeSize - cellSize) * 0.5f;

        gridItems = new bool[cellNumber, cellNumber, cellNumber];
    }

    private void Start()
    {
        // Random initialization!

        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    gridItems[x, y, z] = Random.value > 0.5f;

                    if (gridItems[x, y, z]) activeCells.Add(GetData(x, y, z));
                }
            }
        }

        drawer.Draw(activeCells.ToArray());
    }

    private Vector4 GetData(int x, int y, int z) => new Vector4(origin.x + (x * cellSize), origin.y + (y * cellSize), origin.z + (z * cellSize), cellSize);

    public override void SimulationStep()
    {
        int[] items = Parse3To1(gridItems);

        ComputeBuffer cellBuffer = new ComputeBuffer(items.Length, sizeof(int));
        cellBuffer.SetData(items);
        computeShader.SetBuffer(0, "allCells", cellBuffer);

        computeShader.SetInt("resolution", cellNumber);

        computeShader.SetInt("minNeighboursToSurvive", minNeighboursToSurvive);
        computeShader.SetInt("maxNeighboursToSurvive", maxNeighboursToSurvive);
        computeShader.SetInt("minNeighboursToRevive", minNeighboursToRevive);
        computeShader.SetInt("maxNeighboursToRevive", maxNeighboursToRevive);

        computeShader.Dispatch(0, items.Length / 10, 1, 1);

        cellBuffer.GetData(items);

        gridItems = Parse1To3(items);

        // ==========================

        activeCells.Clear();

        // Apply step states
        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    if (gridItems[x, y, z]) activeCells.Add(GetData(x, y, z));
                }
            }
        }

        if (activeCells.Count > 0) drawer.Draw(activeCells.ToArray());

        cellBuffer.Dispose();
    }

    private int[] Parse3To1(bool[,,] items)
    {
        int[] res = new int[cellNumber * cellNumber * cellNumber];

        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    res[x * cellNumber * cellNumber + y * cellNumber + z] = items[x, y, z] ? 1 : 0;
                }
            }
        }

        return res;
    }

    private bool[,,] Parse1To3(int[] items)
    {
        bool[,,] res = new bool[cellNumber, cellNumber, cellNumber];

        for (int i = 0; i < items.Length; i++)
        {
            res[(i / cellNumber) / cellNumber % cellNumber, (i / cellNumber) % cellNumber, i % cellNumber] = items[i] > 0;
        }

        return res;
    }
}
