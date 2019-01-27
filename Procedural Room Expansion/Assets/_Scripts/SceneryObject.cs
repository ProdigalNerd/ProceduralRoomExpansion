using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryObject : MonoBehaviour {

    public enum SceneryDirection { Top, Right, Bottom, Left, None }
    public enum SceneryType { Floor, Wall, DoorInside, Locked, DoorOutside }

    public int width;
    public int height;
    public int layerIndex = 0;
    public SceneryDirection direction;
    public SceneryType type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
