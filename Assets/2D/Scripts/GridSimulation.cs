using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridSimulation : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GridPoint prefab;
    [Header("Simuation Parameters")]
    [SerializeField] int minNeighboursToSurvive = 2;
    [SerializeField] int maxNeighboursToSurvive = 3;
    [SerializeField] int minNeighboursToRevive = 3;
    [SerializeField] int maxNeighboursToRevive = 3;

    private List<List<GridPoint>> gridItems = new List<List<GridPoint>>();
    private List<List<bool>> resultItems = new List<List<bool>>();

    private void Awake()
    {
        InitalizeGrid(gridItems);

        InitalizeGrid(resultItems);
    }

    private void Start()
    {
        for (int x = 0; x < grid.X; x++)
        {
            for (int y = 0; y < grid.Y; y++)
            {
                Vector2 coordinates = new Vector2(x, y);
                GridPoint instance = Instantiate(prefab, transform);

                gridItems[x][y] = instance;

                grid.PositionItemOnGrid(instance.transform, coordinates);
            }
        }
    }

    public void SimulationStep()
    {
        // Fill result with current info
        for (int x = 0; x < grid.X; x++)
        {
            for (int y = 0; y < grid.Y; y++)
            {
                resultItems[x][y] = gridItems[x][y].Alive;
            }
        }

        //Iterate over every item
        for (int x = 0; x < grid.X; x++)
        {
            for (int y = 0; y < grid.Y; y++)
            {
                int neightbours = GetNeighboursAlive(x, y);

                resultItems[x][y] = neightbours >= (gridItems[x][y].Alive ? minNeighboursToSurvive : minNeighboursToRevive) 
                    && neightbours <= (gridItems[x][y].Alive ? maxNeighboursToSurvive : maxNeighboursToRevive);

                //if (gridItems[x][y].Alive)
                //    resultItems[x][y] = neightbours >= minNeighboursToSurvive && neightbours <= maxNeighboursToSurvive;
                //else
                //    resultItems[x][y] = neightbours >= minNeighboursToRevive && neightbours <= maxNeighboursToRevive;
            }
        }

        // Apply step states
        for (int x = 0; x < grid.X; x++)
        {
            for (int y = 0; y < grid.Y; y++)
            {
                gridItems[x][y].Alive = resultItems[x][y];
            }
        }
    }

    private int GetNeighboursAlive(int x, int y)
    {
        // Ignore edge cases, kill the cell!
        if (x == grid.X - 1 || x == 0 || y == grid.Y - 1 || y == 0)
            return 0;

        int count = 0;

        count += gridItems[x - 1][y + 1].Alive ? 1 : 0;
        count += gridItems[x][y + 1].Alive ? 1 : 0;
        count += gridItems[x + 1][y + 1].Alive ? 1 : 0;

        count += gridItems[x - 1][y].Alive ? 1 : 0;
        count += gridItems[x + 1][y].Alive ? 1 : 0;

        count += gridItems[x - 1][y - 1].Alive ? 1 : 0;
        count += gridItems[x][y - 1].Alive ? 1 : 0;
        count += gridItems[x + 1][y - 1].Alive ? 1 : 0;

        return count;
    }

    private void InitalizeGrid<T>(List<List<T>> grid)
    {
        for (int x = 0; x < this.grid.X; x++)
        {
            grid.Add(new List<T>());

            for (int y = 0; y < this.grid.Y; y++)
            {
                grid[x].Add(default(T));
            }
        }
    }
}
