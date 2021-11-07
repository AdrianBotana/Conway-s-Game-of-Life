using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGridSimulation : MonoBehaviour
{
    public abstract void SimulationStep();
}

public class GridSimulation3D : BaseGridSimulation
{
    [SerializeField] GridDrawer3D drawer;
    [Space]
    [SerializeField] float simulationCubeSize;
    [SerializeField] float cellSize;
    [Header("Simuation Parameters")]
    [SerializeField] int minNeighboursToSurvive = 2;
    [SerializeField] int maxNeighboursToSurvive = 3;
    [SerializeField] int minNeighboursToRevive = 3;
    [SerializeField] int maxNeighboursToRevive = 3;

    private List<List<List<bool>>> gridItems = new List<List<List<bool>>>();
    private List<List<List<bool>>> resultItems = new List<List<List<bool>>>();

    private List<Vector4> activeCells = new List<Vector4>();

    private int cellNumber;
    private Vector3 origin;

    private void Awake()
    {
        cellNumber = (int)(simulationCubeSize / cellSize);
        origin = transform.position - Vector3.one * (simulationCubeSize - cellSize) * 0.5f;
    }

    private void Start()
    {
        InitalizeGrid(gridItems);

        InitalizeGrid(resultItems);

        // Random initialization!

        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    gridItems[x][y][z] = Random.value > 0.5f;

                    if (gridItems[x][y][z]) activeCells.Add(GetData(x, y, z));
                }
            }
        }

        drawer.Draw(activeCells.ToArray());
    }

    private Vector4 GetData(int x, int y, int z) => new Vector4(origin.x + (x * cellSize), origin.y + (y * cellSize), origin.z + (z * cellSize), cellSize);


    public override void SimulationStep()
    {
        activeCells.Clear();

        // Fill result with current info
        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    resultItems[x][y][z] = gridItems[x][y][z];
                }
            }
        }

        //Iterate over every item
        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    int neightbours = GetNeighboursAlive(x, y, z);

                    resultItems[x][y][z] = neightbours >= (gridItems[x][y][z] ? minNeighboursToSurvive : minNeighboursToRevive)
                        && neightbours <= (gridItems[x][y][z] ? maxNeighboursToSurvive : maxNeighboursToRevive);

                    if (resultItems[x][y][z]) activeCells.Add(GetData(x, y, z));
                }
            }
        }

        // Apply step states
        for (int x = 0; x < cellNumber; x++)
        {
            for (int y = 0; y < cellNumber; y++)
            {
                for (int z = 0; z < cellNumber; z++)
                {
                    gridItems[x][y][z] = resultItems[x][y][z];
                }
            }
        }

        if (activeCells.Count > 0) drawer.Draw(activeCells.ToArray());
    }

    private int GetNeighboursAlive(int x, int y, int z)
    {
        // Ignore edge cases, kill the cell!
        if (x == cellNumber - 1 || x == 0 || y == cellNumber - 1 || y == 0 || z == cellNumber - 1 || z == 0)
            return 0;

        int count = 0;

        //z-1
        count += gridItems[x - 1][y + 1][z - 1] ? 1 : 0;
        count += gridItems[x][y + 1][z - 1] ? 1 : 0;
        count += gridItems[x + 1][y + 1][z - 1] ? 1 : 0;

        count += gridItems[x - 1][y][z - 1] ? 1 : 0;
        count += gridItems[x][y][z - 1] ? 1 : 0;
        count += gridItems[x + 1][y][z - 1] ? 1 : 0;

        count += gridItems[x - 1][y - 1][z - 1] ? 1 : 0;
        count += gridItems[x][y - 1][z - 1] ? 1 : 0;
        count += gridItems[x + 1][y - 1][z - 1] ? 1 : 0;

        // z
        count += gridItems[x - 1][y + 1][z] ? 1 : 0;
        count += gridItems[x][y + 1][z] ? 1 : 0;
        count += gridItems[x + 1][y + 1][z] ? 1 : 0;

        count += gridItems[x - 1][y][z] ? 1 : 0;
        count += gridItems[x + 1][y][z] ? 1 : 0;

        count += gridItems[x - 1][y - 1][z] ? 1 : 0;
        count += gridItems[x][y - 1][z] ? 1 : 0;
        count += gridItems[x + 1][y - 1][z] ? 1 : 0;

        //z+1
        count += gridItems[x - 1][y + 1][z + 1] ? 1 : 0;
        count += gridItems[x][y + 1][z + 1] ? 1 : 0;
        count += gridItems[x + 1][y + 1][z + 1] ? 1 : 0;

        count += gridItems[x - 1][y][z + 1] ? 1 : 0;
        count += gridItems[x][y][z + 1] ? 1 : 0;
        count += gridItems[x + 1][y][z + 1] ? 1 : 0;

        count += gridItems[x - 1][y - 1][z + 1] ? 1 : 0;
        count += gridItems[x][y - 1][z + 1] ? 1 : 0;
        count += gridItems[x + 1][y - 1][z + 1] ? 1 : 0;

        return count;
    }

    private void InitalizeGrid<T>(List<List<List<T>>> grid)
    {
        for (int x = 0; x < cellNumber; x++)
        {
            grid.Add(new List<List<T>>());

            for (int y = 0; y < cellNumber; y++)
            {
                grid[x].Add(new List<T>());

                for (int z = 0; z < cellNumber; z++)
                    grid[x][y].Add(default(T));
            }
        }
    }
}
