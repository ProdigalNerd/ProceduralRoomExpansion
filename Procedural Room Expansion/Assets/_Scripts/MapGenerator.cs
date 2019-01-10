using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject prefab;
    public int baseNumberOfRooms = 150;
    public int radius = 25;
    public int maxRoomWidth = 8;
    public int maxRoomHeight = 8;

    private List<GameObject> roomList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Create a bunch of "rooms" within the radius of a circle with provided radius
        for (int x = 0; x < baseNumberOfRooms; x++) {
            // need to calculate a random width and height 
            float w = UnityEngine.Random.Range(1, maxRoomWidth);
            float h = UnityEngine.Random.Range(1, maxRoomHeight);
            roomList.Add(CreateQuadAtCoordinate(getRandomPointInCircle(radius), w, h));
        }

        // Quads should automatically spread out
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 getRandomPointInCircle(float rad) {
        return UnityEngine.Random.insideUnitCircle * rad;
    }

    private GameObject CreateQuadAtCoordinate(Vector2 roomLoc, float width, float height) {
        GameObject room = Instantiate(prefab, parentObject.transform) as GameObject;
        room.transform.position = roomLoc;
        room.transform.localScale += new Vector3(width, height, 0);
        // room.GetComponent<BoxCollider>().size = new Vector3(width, height, 0.01f);
        room.transform.parent = parentObject.transform;
        return room;
    }

    private char CalculateHigherDistanceFromCenter(Vector2 box) {
        float distX = Mathf.Abs(box.x);
        float distY = Mathf.Abs(box.y);

        if(distX > distY) {
            return 'x';
        }

        return 'y';
    }

    private void TestCreatePlane() {
        GameObject testobj = Instantiate(prefab, parentObject.transform) as GameObject;
        testobj.transform.position = new Vector2(0, 0);
        testobj.transform.localScale += new Vector3(1, 1, 0);
        testobj.transform.parent = parentObject.transform;
    }
}
