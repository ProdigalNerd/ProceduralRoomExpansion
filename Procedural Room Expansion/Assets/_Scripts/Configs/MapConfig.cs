using System;
using UnityEngine;

namespace Configurations
{
    public class MapConfig : MonoBehaviour
    {
        public GameObject parentObject;

        public int minRadius;
        public int maxRadius;
        public int radiusStepSize;
        public int roomsPerStep;
        public int maxRoomWidth;
        public int maxRoomHeight;
        public int minRoomWidth;
        public int minRoomHeight;

        public void Start()
        {

        }
    }
}