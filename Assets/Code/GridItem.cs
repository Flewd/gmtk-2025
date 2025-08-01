using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Code
{
    public class GridItem : MonoBehaviour
    {
        public enum ItemType { empty, track }
        public GameObject itemInGridSpace;
        public TMP_Text text;
        public ItemType currentItemType;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetItemInGridSpace(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.empty:

                    if (itemInGridSpace != null)
                    {
                        RemoveTrack();
                    }
                    
                    break;
                case ItemType.track:
                    if (currentItemType != ItemType.track)
                    {
                        SpawnTrack();
                    }
                    
                    break;
            }


            currentItemType = itemType;
        }

        public void SpawnTrack()
        {
            itemInGridSpace = PrefabUtility.InstantiatePrefab(Resources.Load("Track"), transform) as GameObject;
            itemInGridSpace.transform.position = transform.position;
#if UNITY_EDITOR
            SceneVisibilityManager.instance.DisablePicking(itemInGridSpace, true);
#endif
        }

        public void RemoveTrack()
        {
            GameObject.Destroy(itemInGridSpace);
            itemInGridSpace = null;
        }
    }
}
