using System;
using UnityEngine;

namespace Classifiers
{
    public static class CornerClassifier
    {
        public static bool IsBottomLeftInnerCorner(in int[,] grid, int x, int y)
        {
            // identify bottom left corner
            /*
             * x x x
             * 1 1 x
             * 0 1 x
             * */
             if(x > 0
                && y > 0
                && grid[x - 1, y] == 1
                && grid[x, y - 1] == 1
                && grid[x - 1, y - 1] == 0
                && grid[x + 1, y + 1] == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsBottomLeftOutterCorner(in int[,] grid, int x, int y)
        {
            // identify bottom left corner
            /*
             * 0 1 1 
             * 0 1 1  
             * 0 0 0
             * */
            if (x > 0
                && y > 0
                && grid[x + 1, y] == 1
                && grid[x, y + 1] == 1
                && grid[x + 1, y + 1] == 1
                && grid[x - 1, y] == 0
                && grid[x, y - 1] == 0
                && grid[x - 1, y - 1] == 0)
            {
                return true;
            }
            /* case where both x and y = 0
             * x x x
             * 1 1 x
             * 1 1 x
             * */
             if (x == 0
                       && y == 0
                       && grid[x + 1, y] == 1
                       && grid[x, y + 1] == 1
                       && grid[x + 1, y + 1] == 1)
            {
                return true;
            }
            /* case where x == 0 or y == 0
             * x x x    x x x x
             * 1 1 x    x 1 1 x
             * 1 1 x    0 1 1 x
             * 0 x x
             * */
            if ((x == 0
                        && y > 0
                        && grid[x + 1, y] == 1
                        && grid[x, y + 1] == 1
                        && grid[x + 1, y + 1] == 1
                        && grid[x, y - 1] == 0)
                    || (y == 0
                        && x > 0
                        && grid[x + 1, y] == 1
                        && grid[x, y + 1] == 1
                        && grid[x + 1, y + 1] == 1
                        && grid[x - 1, y] == 0))
            {
                return true;
            }

            return false;
        }

        public static bool IsBottomRightInnerCorner(in int[,] grid, int x, int y)
        {
            // identify bottom right inner corner
            /*
             * x x x
             * x 1 1
             * x 1 0
             * */
            if (x < grid.GetLength(0) - 1
               && y > 0
               && grid[x + 1, y] == 1
               && grid[x, y - 1] == 1
               && grid[x + 1, y - 1] == 0
               && grid[x - 1, y + 1] == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsBottomRightOutterCorner(in int[,] grid, int x, int y)
        {
            // identify bottom right outter corner
            /*
             * x 1 0
             * 1 1 0
             * 0 0 0
             * */
             if(x > 0
                && y < grid.GetLength(1) - 1
                && grid[x - 1, y] == 1
                && grid[x, y + 1] == 1
                && grid[x + 1, y - 1] == 0
                && grid[x + 1, y] == 0
                && grid[x, y - 1] == 0)
            {
                return true;
            }

             /* case where y = grid.GetLength(1) - 1 and x == 0
              * x x x
              * x x 1
              * x 1 1
              * */
            if(x == 0
                && y == grid.GetLength(1) - 1
                && grid[x, y + 1] == 1
                && grid[x - 1, y] == 1)
            {
                return true;
            }

            /*
             * x x x    x x x x            
             * x 1 1    x 1 1 x            
             * x 1 1    x 1 1 0
             * x x 0
             * */            
            if((x == grid.GetLength(0) - 1
                && y > 0
                && grid[x - 1, y] == 1
                && grid[x, y + 1] == 1
                && grid[x - 1, y + 1] == 1
                && grid[x, y - 1] == 0)
            || (x < grid.GetLength(0) - 1
                && y == 0
                && y == grid.GetLength(1) - 1
                && grid[x - 1, y] == 1
                && grid[x, y + 1] == 1
                && grid[x + 1, y] == 0))
            {
                return true;
            }

            return false;
        }

        public static bool IsTopLeftInnerCorner(in int[,] grid, int x, int y)
        {
            // identify top left inner corner
            /*
             * 0 1 x
             * 1 1 x
             * x x 1
             * */
             if(x > 0
                && y < grid.GetLength(1) - 1
                && grid[x - 1, y] == 1
                && grid[x, y + 1] == 1
                && grid[x + 1, y - 1] == 1
                && grid[x - 1, y + 1] == 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsTopLeftOutterCorner(in int[,] grid, int x, int y)
        {
            // identify top left outer corner
            /*
             * 0 0 x
             * 0 1 1
             * x 1 1
             * */
             if(x > 0
                && y < grid.GetLength(1) - 1
                && grid[x + 1, y] == 1
                && grid[x, y - 1] == 1
                && grid[x + 1, y - 1] == 1
                && grid[x - 1, y] == 0
                && grid[x, y + 1] == 0
                && grid[x - 1, y + 1] == 0)
            {
                return true;
            }

             /* case where x == 0 and y == grid.GetLength(1) - 1
              * 1 1 x
              * 1 1 x
              * x x x
              * */
             if(x == 0
                && y == grid.GetLength(1) - 1
                && grid[x + 1, y] == 1
                && grid[x, y - 1] == 1
                && grid[x + 1, y - 1] == 1)
            {
                return true;
            }

             /* case where x == 0 OR y == grid.GetLength(1) - 1
              * 0 x x   0 1 1 x           
              * 1 1 x   x 1 1 x
              * 1 1 x   x x x x
              * x x x             
              * */
              if((x == 0
                    && y < grid.GetLength(1) - 1
                    && grid[x, y - 1] == 0
                    && grid[x + 1, y] == 1
                    && grid[x, y - 1] == 1
                    && grid[x + 1, y - 1] == 1)
                || (y == grid.GetLength(1) - 1
                    && x > 0
                    && grid[x - 1, y] == 0
                    && grid[x + 1, y] == 1
                    && grid[x, y - 1] == 1
                    && grid[x + 1, y - 1] == 1))
            {
                return true;
            }

            return false;
        }

        public static bool IsTopRightInnerCorner(in int[,] grid, int x, int y)
        {
            // identify top right inner corner
            /*
             * x 1 0
             * x 1 1
             * 1 x x
             * */
             if(x < grid.GetLength(0) - 1
                && y < grid.GetLength(1) - 1
                && grid[x, y + 1] == 1
                && grid[x + 1, y] == 1
                && grid[x + 1, y + 1] == 0
                && grid[x - 1, y - 1] == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsTopRightOutterCorner(in int[,] grid, int x, int y)
        {
            // identify top right outer corner

            /*
             * x 0 0
             * 1 1 0
             * x 1 x
             * */
             if(x < grid.GetLength(0) - 1
                && y < grid.GetLength(1) - 1
                && grid[x, y + 1] == 0
                && grid[x + 1, y + 1] == 0
                && grid[x + 1, y] == 0
                && grid[x - 1, y] == 1
                && grid[x, y - 1] == 1
                && grid[x - 1, y - 1] == 1)
            {
                return true;
            }

             /* case where both x and y are on the edge
              * x 1 1
              * x 1 1
              * x x x
              * */
              if(x == grid.GetLength(0) - 1
                && y == grid.GetLength(1) - 1
                && grid[x - 1, y] == 1
                && grid[x - 1, y - 1] == 1
                && grid[x, y - 1] == 1)
            {
                return true;
            }

              /* case where x == grid.GetLength(0) - 1 OR y == grid.GetLength(1) - 1
               * x x 0  x 1 1 0
               * x 1 1  x 1 1 x
               * x 1 1  x x x x
               * x x x              
               * */
               if((x == grid.GetLength(0) - 1
                    && y < grid.GetLength(1) - 1
                    && grid[x, y - 1] == 0
                    && grid[x - 1, y] == 1
                    && grid[x - 1, y - 1] == 1
                    && grid[x, y - 1] == 1)
                || (x < grid.GetLength(0) - 1
                    && y == grid.GetLength(1) - 1
                    && grid[x - 1, y] == 1
                    && grid[x + 1, y] == 0
                    && grid[x - 1, y - 1] == 1
                    && grid[x, y - 1] == 1))
            {
                return true;
            }

            return false;
        }
    }
}
