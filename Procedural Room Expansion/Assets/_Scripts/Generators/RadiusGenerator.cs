using System;
using System.Collections.Generic;
using UnityEngine;
using Configurations;

namespace Generators
{
    public class RadiusGenerator
    { 
        private TileConfiguration config;
        private MapConfig mConfig;
        private int[,] grid;
        private int[,] gridCopy;

        public RadiusGenerator(TileConfiguration tileConfiguration,
                                MapConfig mapConfig)
        {
            config = tileConfiguration;
            mConfig = mapConfig;
            grid = new int[mConfig.maxRadius * 2 + mConfig.maxRoomWidth, mConfig.maxRadius * 2 + mConfig.maxRoomHeight];
            gridCopy = new int[mConfig.maxRadius * 2 + mConfig.maxRoomWidth, mConfig.maxRadius * 2 + mConfig.maxRoomHeight];

            gridCopy = grid.Clone() as int[,];
        }

        public void InitializeRooms()
        {
            int prevRadius = 0;

            for(int r = mConfig.minRadius; r <= mConfig.maxRadius; r += mConfig.radiusStepSize)
            {
                if(prevRadius == 0)
                {
                    InitializeNodesForInnerCircle(r);
                }
                else
                {
                    InitializeNodesForOutterCircle(prevRadius, r);
                }

                prevRadius = r;
            }

            ApplyGraphicsToGrid();
        }


        /*
         * Private Members
         * */

        private void UpdateGrid(Vector2 point, int width, int height)
        {
            int posX = Mathf.RoundToInt(point.x) + mConfig.maxRadius; // add max radius so that the lowest possible coordinate is 0
            int posY = Mathf.RoundToInt(point.y) + mConfig.maxRadius; // add max radius so that the lowest possible coordinate is 0

            if (posX >= mConfig.maxRadius * 2) posX = mConfig.maxRadius * 2 - 1;
            if (posY >= mConfig.maxRadius * 2) posY = mConfig.maxRadius * 2 - 1;
            if (posX < 0) posX = 0;
            if (posY < 0) posY = 0;

            for (int y = posY; y < posY + height; y++)
            {
                for (int x = posX; x < posX + width; x++)
                {
                    grid[x, y] = 1;
                }
            }
        }

        private void InitializeNodesForInnerCircle(int radius)
        {
            for (int x = 0; x < mConfig.roomsPerStep; x++)
            {
                // need to calculate a random width and height 
                int width = (int)UnityEngine.Random.Range(mConfig.minRoomWidth, mConfig.maxRoomWidth);
                int height = (int)UnityEngine.Random.Range(mConfig.minRoomHeight, mConfig.maxRoomHeight);

                Vector2 origin = UnityEngine.Random.insideUnitCircle * radius;
                UpdateGrid(origin, width, height);
            }
        }

        private void InitializeNodesForOutterCircle(int prevRadius, int radius)
        {

        }

        private void ApplyGraphicsToGrid()
        {
            ApplyCornerGraphics();
            ApplyWallGraphics();
            ApplyFloorGraphics();
        }

        private void ApplyCornerGraphics()
        {
            // create a copy of grid to store changes after corners are created.
            gridCopy = grid.Clone() as int[,];

            for(int y = 0; y < grid.GetLength(1); y++)
            {
                for(int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == 1)
                    {
                        // used to move the origin point that the graphics will be placed on
                        // also used to help modify the grid once a graphic has been placed
                        Vector2 originModifier = Vector2.zero;

                        // identify bottom left corner
                        if(Classifiers.CornerClassifier.IsBottomLeftInnerCorner(grid, x, y))
                        {
                            CreateObject(config.blInnerCorner, x, y, new Vector2(0, 1), 2);
                        }
                        else if(Classifiers.CornerClassifier.IsBottomLeftOutterCorner(grid, x, y))
                        {
                            CreateObject(config.blOutterCorner, x, y, new Vector2(0, 1), 2);
                        }
                        // identify bottom right corner
                        else if(Classifiers.CornerClassifier.IsBottomRightInnerCorner(grid, x, y))
                        {
                            CreateObject(config.brInnerCorner, x, y, new Vector2(-1, 1), 2);
                        }
                        else if(Classifiers.CornerClassifier.IsBottomRightOutterCorner(grid, x, y))
                        {
                            CreateObject(config.brOutterCorner, x, y, new Vector2(-1, 1), 2);
                        }
                        // identify top left corner
                        else if(Classifiers.CornerClassifier.IsTopLeftInnerCorner(grid, x, y))
                        {
                            CreateObject(config.tlInnerCorner, x, y, new Vector2(0, 0), 2);
                        }
                        else if(Classifiers.CornerClassifier.IsTopLeftOutterCorner(grid, x, y))
                        {
                            CreateObject(config.tlOutterCorner, x, y, new Vector2(0, 0), 2);
                        }
                        // identify top right corner
                        else if(Classifiers.CornerClassifier.IsTopRightInnerCorner(grid, x, y))
                        {
                            CreateObject(config.trInnerCorner, x, y, new Vector2(-1, 0), 2);
                        }
                        else if(Classifiers.CornerClassifier.IsTopRightOutterCorner(grid, x, y))
                        {
                            CreateObject(config.trOutterCorner, x, y, new Vector2(-1, 0), 2);
                        }
                    }
                }
            }

            // merge changes back into the main grid
            grid = gridCopy.Clone() as int[,];
        }

        /*
         * Select and apply graphics to the areas that would represent walls
         * */
        private void ApplyWallGraphics()
        {
            gridCopy = grid.Clone() as int[,];

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if(grid[x, y] == 1)
                    {
                        if(Classifiers.WallClassifier.IsTopWall(grid, x, y))
                        {
                            CreateObject(GetRandomObjectFromList(config.topWalls), x, y, new Vector2(0, 0), 3);
                        }
                        else if(Classifiers.WallClassifier.IsLeftWall(grid, x, y))
                        {
                            CreateObject(GetRandomObjectFromList(config.leftWalls), x, y, new Vector2(0, 0), 3);
                        }
                        else if(Classifiers.WallClassifier.IsRightWall(grid, x, y))
                        {
                            CreateObject(GetRandomObjectFromList(config.rightWalls), x, y, new Vector2(-1, 0), 3);
                        }
                        else if(Classifiers.WallClassifier.IsBottomWall(grid, x, y))
                        {
                            CreateObject(GetRandomObjectFromList(config.bottomWalls), x, y, new Vector2(0, -1), 3);
                        }
                    }
                }
            }

            grid = gridCopy.Clone() as int[,];
        }

        private void ApplyFloorGraphics()
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {

                }
            }
        }

        // Instantiate the object and put it on the screen
        private void CreateObject(GameObject dungeonObj, int x, int y, Vector2 offset, int gridValue)
        {
            GameObject tile = GameObject.Instantiate(dungeonObj, mConfig.parentObject.transform) as GameObject;
            SceneryObject so = tile.GetComponent<SceneryObject>();

            Vector2 position = new Vector2(x, y);

            tile.transform.position = position + offset;

            for (int k = so.height - 1; k >= 0; k--)
            {
                for (int j = 0; j < so.width; j++)
                {
                    gridCopy[(int)tile.transform.position.x + j, (int)tile.transform.position.y - k] = gridValue;
                }
            }
        }

        private GameObject GetRandomObjectFromList(List<GameObject> aryObj)
        {
            int randomIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, aryObj.Count - 1));
            return aryObj[randomIndex];
        }
    }
}
