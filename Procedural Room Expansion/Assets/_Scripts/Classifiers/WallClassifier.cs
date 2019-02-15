using System;
using UnityEngine;

namespace Classifiers
{
    public static class WallClassifier
    {
        // TODO rewrite given that places where corners used to be are represented by 2s

        // Check if the tile being analyzed is going to be a left wall
        public static bool IsLeftWall(in int[,] grid, int x, int y)
        {
            if(x == 0 && grid[x + 1, y] == 1 && grid[x + 2, y] == 1)
            {
                return true;
            }

            if(x < grid.GetLength(0) - 1 && grid[x + 1, y] == 1 && grid[x - 1, y] == 0)
            {
                return true;
            }

            return false;
        }

        // check if the tile being analyzed is going to be a top wall
        public static bool IsTopWall(in int[,] grid, int x, int y)
        {
            if(y == grid.GetLength(1) - 1 && grid[x, y - 1] == 1)
            {
                return true;
            }

            if(y < grid.GetLength(1) - 1 && grid[x, y - 1] == 1 && grid[x, y + 1] == 0)
            {
                return true;
            }

            return false;
        }

        // check if the tile being analyzed is going to be a right wall
        public static bool IsRightWall(in int[,] grid, int x, int y)
        {
            if(x == grid.GetLength(0) - 1 && grid[x - 1, y] == 1)
            {
                return true;
            }

            if(x < grid.GetLength(0) - 1 && grid[x - 1, y] == 1 && grid[x + 1, y] == 0)
            {
                return true;
            }

            return false;
        }

        // check if the tile being analyzed is going to be a bottom wall
        public static bool IsBottomWall(in int[,] grid, int x, int y)
        {
            if(y == 0 && grid[x, y + 1] == 1)
            {
                return true;
            }

            if(y > 0 && grid[x, y + 1] == 1 && grid[x, y - 1] == 0)
            {
                return true;
            }

            return false;
        }
    }
}
