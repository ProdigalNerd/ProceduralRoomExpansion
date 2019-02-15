using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Configurations
{
    public class TileConfiguration : MonoBehaviour
    {
        // Floors
        public List<GameObject> floorTiles;

        // Walls
        public List<GameObject> topWalls;
        public List<GameObject> rightWalls;
        public List<GameObject> bottomWalls;
        public List<GameObject> leftWalls;

        // Corners
        public GameObject tlInnerCorner;
        public GameObject tlOutterCorner;
        public GameObject trInnerCorner;
        public GameObject trOutterCorner;
        public GameObject blInnerCorner;
        public GameObject blOutterCorner;
        public GameObject brInnerCorner;
        public GameObject brOutterCorner;

        // Stairs

        // Doors
    }
}
