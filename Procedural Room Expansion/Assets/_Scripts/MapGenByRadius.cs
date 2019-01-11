using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenByRadius : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject prefab;

    public int minRadius = 5;
    public int maxRadius = 50;
    public int radiusStepSize = 5;
    public int roomsPerStep = 10;
    public int maxRoomWidth = 8;
    public int maxRoomHeight = 8;

    // Start is called before the first frame update
    void Start()
    {
        int previousRadius = 0;
        for (int radius = minRadius; radius <= maxRadius; radius + radiusStepSize) {
            


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsPointWithinCircle(int circleRadius, Vector2 point) {
        return ((Mathf.Abs(point.x) * Mathf.Abs(point.x)) + (Mathf.Abs(point.y) * Mathf.Abs(point.y))) < (circleRadius * circleRadius);
    }
}
