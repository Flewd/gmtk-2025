using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Grid))]
    public class GridCreator : MonoBehaviour
    {
        public Grid grid;

        [Header("Grid Settings")]
        public GameObject gridSquarePrefab;
        [Range(4, 40)] public int gridSizeX = 10;
        [Range(4, 40)] public int gridSizeY = 10;
        private Vector2Int gridSize => new Vector2Int(gridSizeX, gridSizeY);
        private GameObject[,] gridSquares;

        [Header("Track Settings")]
        //public GameObject trackPrefab;

        [Range(4, 40)] public int trackSizeX = 12;
        [Range(4, 40)] public int trackSizeY = 8;
        private Vector2Int trackSize => new Vector2Int(trackSizeX, trackSizeY);
        [Range(0f, 1f)] public float trackVariation = 0.25f;

        [Button]
        public void CreateAll()
        {
            ClearChildren();
            CreateGrid();
            CreateTrack();
        }

        [Button]
        void ClearChildren()
        {
#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                while (transform.childCount > 0)
                {
                    var child = transform.GetChild(0);
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }

                return;
            }
#endif

            while (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                GameObject.Destroy(child.gameObject);
            }

        }


        private void Awake()
        {
            CreateAll();
            grid.Setup();
        }

        void CreateGrid()
        {
            gridSquares = new GameObject[gridSize.x, gridSize.y];

            Vector2 offset = new Vector2(gridSize.x / 2f - 0.5f, gridSize.y / 2f - 0.5f);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 localPos = new Vector3(x - offset.x, 0f, y - offset.y);
                    Vector3 worldPos = transform.position + localPos;

                    GameObject square = null;
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        square = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(gridSquarePrefab, transform);
#endif
                    if (Application.isPlaying)
                        square = GameObject.Instantiate(gridSquarePrefab, transform);

                    square.transform.position = worldPos;
                    square.transform.name = $"Tile_{x}_{y}";
                    gridSquares[x, y] = square;
                }
            }

            grid.gridSize = gridSize;
        }

        void CreateTrack()
        {
            if (trackSize.x < 2 || trackSize.y < 2)
            {
                Debug.LogWarning("Track size must be at least 2x2 to form a valid loop.");
                return;
            }

            // Define the track bounds
            int minX = Mathf.FloorToInt((gridSize.x - trackSize.x) / 2f);
            int minY = Mathf.FloorToInt((gridSize.y - trackSize.y) / 2f);
            int maxX = minX + trackSize.x - 1;
            int maxY = minY + trackSize.y - 1;

            // Define 4 anchor points at the center of each side of the rectangle
            Vector2Int top = new Vector2Int((minX + maxX) / 2, maxY);
            Vector2Int right = new Vector2Int(maxX, (minY + maxY) / 2);
            Vector2Int bottom = new Vector2Int((minX + maxX) / 2, minY);
            Vector2Int left = new Vector2Int(minX, (minY + maxY) / 2);

            // Order of traversal: top → right → bottom → left → top (to close the loop)
            Vector2Int[] anchors = new[] { top, right, bottom, left, top };

            HashSet<Vector2Int> used = new HashSet<Vector2Int>();
            List<Vector2Int> fullPath = new List<Vector2Int>();

            for (int i = 0; i < anchors.Length - 1; i++)
            {
                Vector2Int start = anchors[i];
                Vector2Int end = anchors[i + 1];

                List<Vector2Int> segment = GeneratePathSegment(start, end, used);
                fullPath.AddRange(segment);
            }

            // Instantiate track prefabs on the path
            foreach (var point in fullPath)
            {
                if (point.x < 0 || point.y < 0 || point.x >= gridSize.x || point.y >= gridSize.y)
                    continue;

                GameObject tile = gridSquares[point.x, point.y];
                if (tile != null)
                {
                    var tileItem = tile.GetComponent<GridItem>();
                    tileItem.SetItemInGridSpace(GridItem.ItemType.track, $"Track_{point.x}_{point.y}");
                }
            }
        }

        List<Vector2Int> GeneratePathSegment(Vector2Int start, Vector2Int end, HashSet<Vector2Int> used)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int current = start;
            path.Add(current);
            used.Add(current);

            while (current != end)
            {
                List<Vector2Int> options = new List<Vector2Int>();

                // Decide movement direction
                if (current.x != end.x)
                    options.Add(new Vector2Int(Mathf.Clamp(end.x - current.x, -1, 1), 0));
                if (current.y != end.y)
                    options.Add(new Vector2Int(0, Mathf.Clamp(end.y - current.y, -1, 1)));

                // Add perpendicular options for variation
                if (Random.value < trackVariation)
                {
                    if (current.x != end.x)
                        options.Add(new Vector2Int(0, Random.value > 0.5f ? 1 : -1));
                    if (current.y != end.y)
                        options.Add(new Vector2Int(Random.value > 0.5f ? 1 : -1, 0));
                }

                Shuffle(options);

                bool moved = false;
                foreach (var offset in options)
                {
                    Vector2Int next = current + offset;

                    if (next.x < 0 || next.y < 0 || next.x >= gridSize.x || next.y >= gridSize.y)
                        continue;
                    if (used.Contains(next))
                        continue;

                    path.Add(next);
                    used.Add(next);
                    current = next;
                    moved = true;
                    break;
                }

                if (!moved)
                {
                    // Fallback deterministically toward goal
                    Vector2Int offset = Vector2Int.zero;

                    if (current.x != end.x)
                        offset = new Vector2Int(Mathf.Clamp(end.x - current.x, -1, 1), 0);
                    else if (current.y != end.y)
                        offset = new Vector2Int(0, Mathf.Clamp(end.y - current.y, -1, 1));

                    current += offset;
                    path.Add(current);
                    used.Add(current);
                }
            }

            return path;
        }

        // Shuffle helper
        void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }


        //        void SaveChanges(Object obj)
        //        {
        //#if UNITY_EDITOR
        //            UnityEditor.EditorUtility.SetDirty(obj); // Marks it as modified
        //            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
        //            //UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(obj.scene); // Marks the scene as dirty
        //#endif
        //        }
    }
}
