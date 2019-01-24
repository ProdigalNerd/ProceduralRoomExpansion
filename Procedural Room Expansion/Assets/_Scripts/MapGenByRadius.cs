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
        room.transform.parent = parentObject.transform;
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

        Debug.Log("Min Radius: " + minRadius);
        Debug.Log("Max Radius: " + maxRadius);
        Debug.Log("Num Rooms: " + numRooms);

        for(int x = 0; x < numRooms; x++)
        {
            Vector2 point = GetRandomPointInCircle(maxRadius);

            // if the point is within the inner circle
            if (IsPointWithinCircle(minRadius, point))
            {
                // find the slope of the line that connects the point to the center of the map (0,0)
                // since center point is always 0,0 then we can assume that the slope is always rise / run or y / x
                float slope = point.y / point.x;
                // line then defined by the equation point.y = slope * point.x

                // to get point on the outter circle to ensure that the line intersects, use that equation to then get the point
                float y = slope * (maxRadius + 1);
                // define the outter point
                Vector2 outterPoint = new Vector2(maxRadius + 1, y);

                // Calculate the point where the the line intersects with the edge of the inner circle
                float deltax = point.x - outterPoint.x;
                float deltay = point.y - outterPoint.y;
                float deltar = Mathf.Sqrt((deltax * deltax) + (deltay * deltay));
                float deltap = (point.x * outterPoint.y) - (outterPoint.x * point.y);

                // Get the distance from the intersections to the random point that was created
                // The shortest distance is the direction we need to move the point
                Vector2[] intersections = CalculatePointsUsingDeltas(deltax, deltay, deltar, deltap, minRadius);
                float d1 = CalculateDistanceBetweeinTwoPoints(intersections[0], point);
                float d2 = CalculateDistanceBetweeinTwoPoints(intersections[1], point);

                // set the direction vector based on shortest distance
                Vector2 targetDirection = intersections[0];
                if (d2 < d1) targetDirection = intersections[1];

                // calculate the distance ratio
                float distanceFromCenter = CalculateDistanceBetweeinTwoPoints(Vector2.zero, targetDirection);
                float innerRatio = distanceFromCenter / minRadius;

                float newX = minRadius + (innerRatio * (maxRadius - minRadius));
                float newY = slope * (newX);

                point = new Vector2(newX, newY);
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
