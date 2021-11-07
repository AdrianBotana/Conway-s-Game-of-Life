using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridComputerShaderSimulation : BaseGridSimulation
{
    [SerializeField] GridDrawer3D drawer;
    [SerializeField] ComputeShader computeShader;
    [Header("Simuation Parameters")]
    [SerializeField] bool fillOnStart;
    [SerializeField] int minNeighboursToSurvive = 2;
    [SerializeField] int maxNeighboursToSurvive = 3;
    [SerializeField] int minNeighboursToRevive = 3;
    [SerializeField] int maxNeighboursToRevive = 3;

    private int[] gridItems;

    private List<Vector4> activeCells = new List<Vector4>();

    private int cellNumber;
    private Vector3 origin;

    private void Awake()
    {
        cellNumber = (int)(Grid3D.GridSize / Grid3D.CellSize);
        origin = transform.position - Vector3.one * (Grid3D.GridSize - Grid3D.CellSize) * 0.5f;

        gridItems = new int[cellNumber * cellNumber * cellNumber];
    }

    private void Start()
    {
        if (!fillOnStart) return;

        for (int i = 0; i < gridItems.Length; i++)
        {
            bool value = Random.value > 0.5f;
            gridItems[i] = value ? 1 : 0;
            if(value) activeCells.Add(GetData(i));
        }

        drawer.Draw(activeCells.ToArray());
    }

    public void SwitchCellState(int x, int y, int z, bool active) 
    {
        int index = x * cellNumber * cellNumber + y * cellNumber + z;

        if (index < gridItems.Length) gridItems[index] = active ? 1 : 0;
    }

    public override void SimulationStep()
    {
        ComputeBuffer cellBuffer = new ComputeBuffer(gridItems.Length, sizeof(int));
        cellBuffer.SetData(gridItems);
        computeShader.SetBuffer(0, "allCells", cellBuffer);

        computeShader.SetInt("resolution", cellNumber);

        computeShader.SetInt("minNeighboursToSurvive", minNeighboursToSurvive);
        computeShader.SetInt("maxNeighboursToSurvive", maxNeighboursToSurvive);
        computeShader.SetInt("minNeighboursToRevive", minNeighboursToRevive);
        computeShader.SetInt("maxNeighboursToRevive", maxNeighboursToRevive);

        computeShader.Dispatch(0, gridItems.Length / 10, 1, 1);

        cellBuffer.GetData(gridItems);

        // ==========================

        activeCells.Clear();

        for (int i = 0; i < gridItems.Length; i++)
            if (gridItems[i] > 0) activeCells.Add(GetData(i));


        if (activeCells.Count > 0) drawer.Draw(activeCells.ToArray());

        cellBuffer.Dispose();
    }

    private Vector4 GetData(int i)
    {
        int z = i % cellNumber;
        int y = (i / cellNumber) % cellNumber;
        int x = ((i / cellNumber) / cellNumber) % cellNumber;

        return new Vector4(origin.x + (x * Grid3D.CellSize), origin.y + (y * Grid3D.CellSize), origin.z + (z * Grid3D.CellSize), Grid3D.CellSize);
    }
}
