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

                    grid[x, y].text.text = $"{x},{y}";

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
                index.x >= gridSize.x)
            {
                return null;
            }
            if (index.y < 0 ||
                index.y >= gridSize.y)
            {
                return null;
            }

            var gridItem = grid[Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y)];

            //Debug.Log($"Getting griditem at {index.x},{index.y} gives out {gridItem.name}");
            return gridItem;
        }

        /// <summary>
        /// Gets a neighboring tile with a track. searches clockwise from the starting direction
        /// </summary>
        /// <param name="startIndex">grid index to search from</param>
        /// <param name="startDirection">starting direction to check first</param>
        /// <returns></returns>
        public Vector2 ConnectingTrack(Vector2 startIndex, Vector2 startDirection)
        {
            //Debug.Log("------Check ConnectingTrack ---------------------");
            var currentGridItem = Get(startIndex);

            // check forward
            var target = startIndex + startDirection;
            var targetItem = Get(target);

            //var resultString = targetItem != null && targetItem.currentItemType == GridItem.ItemType.track ? "FOUND" : "NOT";
            //Debug.Log($"(Forward {startDirection.ToString()}) currently at {startIndex.x},{startIndex.y}, fetching {target.x},{target.y} - {resultString}");

            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }

            // check left
            var leftDir = TurnDirLeft(startDirection);
            target = startIndex + leftDir;
            targetItem = Get(target);

            //resultString = targetItem != null && targetItem.currentItemType == GridItem.ItemType.track ? "FOUND" : "NOT";
            //Debug.Log($"(Left {leftDir.ToString()}) currently at {startIndex.x},{startIndex.y}, fetching {target.x},{target.y} - {resultString}");

            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }


            //check right
            var rightDir = TurnDirRight(startDirection);
            target = startIndex + rightDir;
            targetItem = Get(target);

            //resultString = targetItem != null && targetItem.currentItemType == GridItem.ItemType.track ? "FOUND" : "NOT";
            //Debug.Log($"(Right {rightDir.ToString()}) currently at {startIndex.x},{startIndex.y}, fetching {target.x},{target.y} - {resultString}");

            if (targetItem != null &&
                targetItem.currentItemType == GridItem.ItemType.track)
            {
                return target;
            }



            // check backwards last
            var backDir = (startDirection * -1);
            target = startIndex + backDir;
            targetItem = Get(target);

            //resultString = targetItem != null && targetItem.currentItemType == GridItem.ItemType.track ? "FOUND" : "NOT";
            //Debug.Log($"(Back {backDir.ToString()}) currently at {startIndex.x},{startIndex.y}, fetching {target.x},{target.y} - {resultString}");

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
