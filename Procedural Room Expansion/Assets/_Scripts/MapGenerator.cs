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
    public int mainRoomMinWidth = 4;
    public int mainRoomMinHeight = 4;

    private List<GameObject> roomList = new List<GameObject>();
    private List<GameObject> processedRooms = new List<GameObject>();
    private int numFinished = 0;
    private bool isSettingUp = true;
    private bool roomsPositioned = false;

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
        if (isSettingUp)
        {
            foreach (GameObject room in roomList)
            {
                bool finished = room.GetComponent<ColliderBasedReposition>().doneMoving;

                if(finished){
                    processedRooms.Add(room);
                    roomList.Remove(room);
                    break;
                }
            }

            if(numFinished == roomList.Count) {
                isSettingUp = false;
            }
        }
        else {
            if(!roomsPositioned) {
                roomsPositioned = true;

                FindMainRooms();
            }
        }
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

    private void FindMainRooms() {
        // Locate the "main rooms" to be used as a foundation
        Debug.Log("Find Main Rooms");
        foreach(GameObject room in processedRooms) {
            if (room.transform.localScale.x >= mainRoomMinWidth && room.transform.localScale.y >= mainRoomMinHeight)
            {
                room.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
        }
    }
}
