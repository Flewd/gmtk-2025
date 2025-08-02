using System.Collections;
using UnityEngine;

namespace Code
{
    public class TrainController : MonoBehaviour
    {
        public Grid grid;

        public Vector2 trackIndex;
        public GridItem currentTrack;

        public Vector2 targetTrackIndex;
        public GridItem targetTrack;

        public float trainSpeed = 10;
        public int turnSpeed = 30;


        void Start()
        {
            SetClosestTrack();
            NextTrack();
        }



        // Update is called once per frame
        void Update()
        {
            var distance = Vector3.Distance(transform.position, targetTrack.transform.position + Vector3.up);

            if (distance < 0.1f)
            {
                trackIndex = targetTrackIndex;
                currentTrack = targetTrack;
                NextTrack();
            }
            //  else if (distance < 1)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Util.NormalizedDirection(currentTrack.transform.position, targetTrack.transform.position)), turnSpeed * Time.deltaTime);
            }
            transform.position = Vector3.MoveTowards(transform.position, targetTrack.transform.position + Vector3.up, trainSpeed * Time.deltaTime);
        }

        private void NextTrack()
        {
            var dir = transform.forward;
            //Debug.Log($"Train Forward {dir.ToString()}");
            Vector2 indexDirection = Vector2.zero;

            if (dir.x < -0.5f)  // left
            {
                indexDirection.x = -1;
                indexDirection.y = 0;
            }
            else if (dir.x > 0.5f)  // right
            {
                indexDirection.x = 1;
                indexDirection.y = 0;
            }
            else if (dir.z < -0.5f) // up
            {
                indexDirection.x = 0;
                indexDirection.y = -1;
            }
            else if (dir.z > 0.5f) // down
            {
                indexDirection.x = 0;
                indexDirection.y = 1;
            }

            targetTrackIndex = grid.ConnectingTrack(trackIndex, indexDirection);
            targetTrack = grid.Get(targetTrackIndex);
        }

        private void SetClosestTrack()
        {
            GridItem closest = null;
            float closestDistance = float.MaxValue;
            Vector2 trackIndex = Vector2.zero;

            for (int x = 0; x < grid.gridSize.x; x++)
            {
                for (int y = 0; y < grid.gridSize.y; y++)
                {
                    var item = grid.grid[x, y];
                    if (item.currentItemType == GridItem.ItemType.track)
                    {
                        var distance = Vector3.Distance(transform.position, item.transform.position);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closest = item;
                            trackIndex.x = x;
                            trackIndex.y = y;
                        }
                    }

                }
            }

            currentTrack = closest;
            this.trackIndex = trackIndex;


        }

    }
}
