using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int X { get { return horizontal; } }
    public int Y { get { return vertical; } }

    [SerializeField, Range(5, 100)] int horizontal;
    [SerializeField, Range(5, 100)] int vertical;

    //Corner locations in world coordinates
    Vector3 upperLeft;
    Vector3 upperRight;
    Vector3 lowerLeft;
    Vector3 lowerRight;

    float horizontalDistance;
    float verticalDistance;

    float horizontalIncrement;
    float verticalIncrement;

    private void Awake()
    {
        GetData();
    }

    public void PositionItemOnGrid(Transform item, Vector2 position)
    {
        item.transform.position = upperLeft + Vector3.right * horizontalIncrement * (int)position.x + Vector3.down * verticalIncrement * (int)position.y + Vector3.right * horizontalIncrement * 0.5f + Vector3.down * verticalIncrement * 0.5f;
        item.localScale = new Vector3(horizontalIncrement, verticalIncrement, 1);
    }

    private void GetData()
    {
        //Corner locations in world coordinates
        upperLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10));
        upperRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));
        lowerLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        lowerRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10));

        horizontalDistance = Vector3.Distance(upperLeft, upperRight);
        verticalDistance = Vector3.Distance(upperLeft, lowerLeft);

        horizontalIncrement = horizontalDistance / horizontal;
        verticalIncrement = verticalDistance / vertical;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        GetData();

        // Draw grid
        for (float x = 0; x < horizontalDistance; x += horizontalIncrement)
        {
            Gizmos.DrawLine(upperLeft + Vector3.right * x, lowerLeft + Vector3.right * x);
        }

        for (float y = 0; y < verticalDistance; y += verticalIncrement)
        {
            Gizmos.DrawLine(lowerLeft + Vector3.up * y, lowerRight + Vector3.up * y);
        }
    }
}
