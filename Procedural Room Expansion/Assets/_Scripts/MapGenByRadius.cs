using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenByRadius : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject prefab;

    public int minRadius = 5;
    public int maxRadius = 10;
    public int radiusStepSize = 5;
    public int roomsPerStep = 10;
    public int maxRoomWidth = 8;
    public int maxRoomHeight = 8;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Use UnityEngine.Random to find a random point within circle
    private Vector2 GetRandomPointInCircle(float rad)
    {
        return UnityEngine.Random.insideUnitCircle * rad;
    }

    // calculate the distance between two points
    private float CalculateDistanceBetweeinTwoPoints(Vector2 point1, Vector2 point2)
    {
        float d = Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2));

        return d;
    }

    // Mainly used to make sure point is in outer rim and not previous circle
    private bool IsPointWithinCircle(int circleRadius, Vector2 point) {
        return ((Mathf.Abs(point.x) * Mathf.Abs(point.x)) + (Mathf.Abs(point.y) * Mathf.Abs(point.y))) < (circleRadius * circleRadius);
    }

    // Create the game object
    private GameObject CreateQuadAtCoordinate(Vector2 roomLoc, float width, float height)
    {
        GameObject room = Instantiate(prefab, parentObject.transform) as GameObject;
        room.transform.position = roomLoc;
        room.transform.localScale += new Vector3(width, height, 0);
        // room.GetComponent<BoxCollider>().size = new Vector3(width, height, 0.01f);
        room.transform.SetParent(parentObject.transform);
        return room;
    }

    // Generating inside of the inner-most circle. aka. easy mode.
    private List<GameObject> GenerateRoomsForInnerCircle(int radius, int numRooms)
    {
        List<GameObject> rmList = new List<GameObject>();

        for(int x = 0; x < numRooms; x++)
        {
            // need to calculate a random width and height 
            float w = UnityEngine.Random.Range(1, maxRoomWidth);
            float h = UnityEngine.Random.Range(1, maxRoomHeight);
            Vector2 origin = GetRandomPointInCircle(radius);
            rmList.Add(CreateQuadAtCoordinate(origin, w, h));
        }

        // Make sure there are no overlaps.


        // Return results
        return rmList;
    }

    private List<GameObject> GenerateRoomsForOutterCircles(int minRadius, int maxRadius, int numRooms)
    {
        List<GameObject> rmList = new List<GameObject>();

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

            rmList.Add(CreateQuadAtCoordinate(point, w, h));
        }

        // Return results
        return rmList;
    }

    // To better understand how this works follow this link
    // http://mathworld.wolfram.com/Circle-LineIntersection.html
    private Vector2[] CalculatePointsUsingDeltas(float deltax, float deltay, float deltar, float deltap, float radius)
    {
        int modifier = 1;
        if (deltay < 0) modifier = -1;

        float xplus, xminus, yplus, yminus;

        // find the x values first
        xplus = ((deltap * deltay) + (modifier * deltax) * Mathf.Sqrt(((radius * radius) * (deltar * deltar)) - (deltap * deltap))) / (deltar * deltar);
        xminus = ((deltap * deltay) - (modifier * deltax) * Mathf.Sqrt(((radius * radius) * (deltar * deltar)) - (deltap * deltap))) / (deltar * deltar);

        // find the y values now
        yplus = ((-1 * deltap * deltax) + Mathf.Abs(deltay) * Mathf.Sqrt(((radius * radius) * (deltar * deltar)) - (deltap * deltap))) / (deltar * deltar);
        yminus = ((-1 * deltap * deltax) - Mathf.Abs(deltay) * Mathf.Sqrt(((radius * radius) * (deltar * deltar)) - (deltap * deltap))) / (deltar * deltar);

        Vector2[] pointArray =
        {
            new Vector2(xplus,yplus),
            new Vector2(xminus,yminus)
        };

        return pointArray;
    }
}
