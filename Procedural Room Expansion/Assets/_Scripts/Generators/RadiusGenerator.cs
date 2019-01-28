using System;
using UnityEngine;

namespace Generators
{
    public class RadiusGenerator
    {
        private TileConfiguration config;
        private int w;
        private int h;
        private int[,] grid;

        public RadiusGenerator(TileConfiguration tileConfiguration,
                                int gridWidth,
                                int gridHeight)
        {
            config = tileConfiguration;
            w = gridWidth;
            h = gridHeight;
            rps = roomsPerStep;
            grid = new int[w, h];
        }

        public void InitializeRooms(int minRadius, 
                                    int maxRadius, 
                                    int step, 
                                    int roomsPerStep)
        {
            int prevRadius = 0;

            for(int r = minRadius; r <= maxRadius; r += step)
            {
                if(prevRadius == 0)
                {

                }

                prevRadius = r;
            }
        }


        /*
         * Private Members
         * */

        private void InitializeNodesForInnerCircle(int radius, int rps)
        {

        }
    }
}
