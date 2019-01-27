using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenByRadius : MonoBehaviour
{
    public GameObject parentObject;
    public TileConfiguration tileConfiguration;

    public int minRadius = 5;
    public int maxRadius = 10;
    public int radiusStepSize = 5;
    public int roomsPerStep = 10;
    public int maxRoomWidth = 8;
    public int maxRoomHeight = 8;

    private int[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[maxRadius * 2 + maxRoomWidth, maxRadius * 2 + maxRoomWidth];

        int previousRadius = 0;
        for (int radius = minRadius; radius <= maxRadius; radius += radiusStepSize) {
            if(previousRadius == 0)
            {
                GenerateRoomsForInnerCircle(radius, roomsPerStep);
            }
            else
            {
                GenerateRoomsForOutterCircles(previousRadius, radius, roomsPerStep);
            }

            previousRadius = radius;
        }

        AddTilesToGrid();
    }

    // Use UnityEngine.Random to find a random point within circle
    private Vector2 GetRandomPointInCircle(float rad)
    {
        return UnityEngine.Random.insideUnitCircle * rad;
    }

    // calculate the distance between two points
    private float CalculateDistanceBetweeinTwoPoints(Vector2 point1, Vector2 point2)
    {
        return Vector2.Distance(point1, point2);
    }

    // Mainly used to make sure point is in outer rim and not previous circle
    private bool IsPointWithinCircle(int circleRadius, Vector2 point) {
        return ((Mathf.Abs(point.x) * Mathf.Abs(point.x)) + (Mathf.Abs(point.y) * Mathf.Abs(point.y))) < (circleRadius * circleRadius);
    }

    private void UpdateGrid(Vector2 point, float w, float h)
    {
        int width = Mathf.RoundToInt(w);
        int height = Mathf.RoundToInt(h);
        int posX = Mathf.RoundToInt(point.x) + maxRadius; // add max radius so that the lowest possible coordinate is 0
        int posY = Mathf.RoundToInt(point.y) + maxRadius; // add max radius so that the lowest possible coordinate is 0

        if (posX >= maxRadius * 2) posX = maxRadius * 2 - 1;
        if (posY >= maxRadius * 2) posY = maxRadius * 2 - 1;
        if (posX < 0) posX = 0;
        if (posY < 0) posY = 0;

        for(int y = posY; y < posY + height; y++)
        {
            for(int x = posX; x < posX + width; x++)
            {
                grid[x, y] = 1;
            }
        }
    }

    // Generating inside of the inner-most circle. aka. easy mode.
    private void GenerateRoomsForInnerCircle(int radius, int numRooms)
    {
        List<GameObject> rmList = new List<GameObject>();

        for(int x = 0; x < numRooms; x++)
        {
            // need to calculate a random width and height 
            float w = UnityEngine.Random.Range(1, maxRoomWidth);
            float h = UnityEngine.Random.Range(1, maxRoomHeight);
            Vector2 origin = GetRandomPointInCircle(radius);
            UpdateGrid(origin, w, h);
        }
    }

    private void GenerateRoomsForOutterCircles(int minRadius, int maxRadius, int numRooms)
    {
        for(int x = 0; x < numRooms; x++)
        {
            Vector2 point = GetRandomPointInCircle(maxRadius);

            // if the point is within the inner circle
            if (IsPointWithinCircle(minRadius, point))
            {
                Vector2 direction = point - Vector2.zero;
                // calculate the distance ratio
                float distanceFromCenter = CalculateDistanceBetweeinTwoPoints(Vector2.zero, direction);
                float innerRatio = distanceFromCenter / minRadius;
                Vector2 finalDirection = direction + direction.normalized * (innerRatio * (maxRadius - minRadius));

                point = point + finalDirection;
            }

            // need to calculate a random width and height 
            float w = UnityEngine.Random.Range(1, maxRoomWidth);
            float h = UnityEngine.Random.Range(1, maxRoomHeight);

            UpdateGrid(point, w, h);
        }
    }

    private void AddTilesToGrid()
    {
        for(int y = 0; y < grid.GetLength(1) - 1; y++)
        {
            for(int x = 0; x < grid.GetLength(0) - 1; x++)
            {
                // 1 == dungeon tile, 0 == empty space
                if(grid[x, y] == 1)
                {
                    GameObject randomWall = new GameObject();

                    if(y == 0 || grid[x, y - 1] == 0)
                    {
                        // top wall
                        randomWall = GetRandomObjectFromList(tileConfiguration.topWalls);
                    }
                    else if(x == grid.GetLength(0) - 1 || grid[x + 1, y] == 0)
                    {
                        // right wall
                        randomWall = GetRandomObjectFromList(tileConfiguration.rightWalls);
                    }
                    else if(y == grid.GetLength(1) - 1 || grid[x, y + 1] == 0)
                    {
                        // bottom wall
                        randomWall = GetRandomObjectFromList(tileConfiguration.bottomWalls);
                    }
                    else if(x == 0 || grid[x - 1, y] == 0)
                    {
                        // left wall
                        randomWall = GetRandomObjectFromList(tileConfiguration.leftWalls);
                    }

                    SceneryObject so = randomWall.GetComponent<SceneryObject>();

                    int offX = 0;
                    int offY = 0;

                    if (so.direction == SceneryObject.SceneryDirection.Top) offY = so.height - 1;
                    if (so.direction == SceneryObject.SceneryDirection.Left) offX = so.width - 1;

                    InstantiateObject(randomWall, new Vector2((x + offX) - maxRadius, (y + offY) - maxRadius));
                }
                else
                {

                }
            }
        }
    }

    public GameObject GetRandomObjectFromList(List<GameObject> aryObj)
    {
        int randomIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, aryObj.Count));
        return aryObj[randomIndex];
    }

    public void InstantiateObject(GameObject obj, Vector2 pos)
    {
        GameObject nObj = Instantiate(obj, parentObject.transform) as GameObject;
        nObj.transform.position = pos;
        nObj.transform.SetParent(parentObject.transform);
    }
}
