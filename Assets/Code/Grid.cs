using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Code
{
    public class Grid : MonoBehaviour
    {
        public Vector2Int gridSize;
        public GridItem[,] grid;
        public GridItem startGridItem;



        public void Setup()
        {
            startGridItem = transform.GetChild(0).GetComponent<GridItem>();

            SetGridSize();
            InitGrid();
        }

        private void InitGrid()
        {
            Vector3 start = startGridItem.transform.position + Vector3.up;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {

                    int index = (int)(gridSize.y * x + y);
                    grid[x, y] = transform.GetChild(index).GetComponent<GridItem>();


                    Debug.Log($"{x},{y} = {grid[x, y].name}");

                }
            }


        }

        private void SetGridSize()
        {
            grid = new GridItem[gridSize.x, gridSize.y];           
        }

        public GridItem Get(Vector2 index)
        {
            if (index.x < 0 ||
                index.x >= grid.GetLength(0))
            {
                return null;
            }
            if (index.y < 0 ||
                index.y >= grid.GetLength(1))
            {
                return null;
            }
            return grid[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)];
        }

        /// <summary>
        /// Gets a neighboring tile with a track. searches clockwise from the starting direction
        /// </summary>
        /// <param name="startIndex">grid index to search from</param>
        /// <param name="startDirection">starting direction to check first</param>
        /// <returns></returns>
        public Vector2 ConnectingTrack(Vector2 startIndex, Vector2 startDirection)
        {
            // check forward
            var target = startIndex + startDirection;
            var targetItem = Get(target);
            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }

            // check left
            target = startIndex + TurnDirLeft(startDirection);
            targetItem = Get(target);
            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }

            //check right
            target = startIndex + TurnDirRight(startDirection);
            targetItem = Get(target);
            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }

            // check backwards last
            target = startIndex + (startDirection * -1);
            targetItem = Get(target);
            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }

            Debug.LogError($"Couldn't find connecting track from {startIndex}");
            return startIndex;
        }

        private Vector2 TurnDirRight(Vector2 dir)
        {
            if (dir.x > 0.5f)
            {
                return new Vector2(0, 1);
            }
            else if (dir.y > 0.5f)
            {
                return new Vector2(-1, 0);
            }
            else if (dir.x < -0.5f)
            {
                return new Vector2(0, -1);
            }
            else if (dir.y < -0.5f)
            {
                return new Vector2(1, 0);
            }

            Debug.LogError($"Somehow TurnDirRight couldn't change directions: {dir}");
            return dir;
        }

        private Vector2 TurnDirLeft(Vector2 dir)
        {
            if (dir.x > 0.5f)
            {
                return new Vector2(0, -1);
            }
            else if (dir.y > 0.5f)
            {
                return new Vector2(1, 0);
            }
            else if (dir.x < -0.5f)
            {
                return new Vector2(0, 1);
            }
            else if (dir.y < -0.5f)
            {
                return new Vector2(-1, 0);
            }
            Debug.LogError($"Somehow TurnDirLeft couldn't change directions: {dir}");
            return dir;
        }
    }
}
