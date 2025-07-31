using System.Linq;
using UnityEngine;

namespace Code
{
    public class Grid : MonoBehaviour
    {
        public Transform[,] grid;

        public Transform startGridItem;
    
        void Start()
        {
            SetGridSize();
            InitGrid();
        }

        private void InitGrid()
        {
            Vector3 start = startGridItem.position + Vector3.up;
        
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    var pos = start + (Vector3.right * x) + (Vector3.back * y);
                    if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 3))
                    {
                        grid[x, y] = hit.transform;
                        hit.transform.GetComponent<GridItem>().text.text = $"{x},{y}";
                    }
                }
            }

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Debug.Log($"{x},{y}={grid[x,y].name}={grid[x,y].position}" );
                }
            }

        }
    
        private void SetGridSize()
        {
            var gridSquares = GetComponentsInChildren<Transform>();
            var xSorted = gridSquares.OrderBy(t => t.position.x).ToList();
        
            float lowestX = float.MaxValue;
            float highestX = float.MinValue;
            float lowestZ = float.MaxValue;
            float highestZ = float.MinValue;
        
            foreach (var t in xSorted)
            {
                if (t.position.x < lowestX)
                {
                    lowestX = t.position.x;
                }
            
                if (t.position.x > highestX)
                {
                    highestX = t.position.x;
                }
            
                if (t.position.z > highestZ)
                {
                    highestZ = t.position.z;
                }
                if (t.position.z < lowestZ)
                {
                    lowestZ = t.position.z;
                }
            }

            int xCount = (int)Mathf.Abs(highestX - lowestX) + 1;
            int yCount = (int)Mathf.Abs(highestZ - lowestZ) + 1;

            grid = new Transform[xCount,yCount];

            Debug.Log(xCount + " " + yCount);
        
        }
    
        void Update()
        {
        
        }
    }
}
