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

        public GameObject SetItemInGridSpace(ItemType itemType, string name = "")
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

            if (itemInGridSpace != null && !string.IsNullOrEmpty(name))
                itemInGridSpace.name = name;

            return itemInGridSpace;
        }

        public void SpawnTrack()
        {
            itemInGridSpace = PrefabUtility.InstantiatePrefab(Resources.Load("Track"), transform) as GameObject;
            itemInGridSpace.transform.position = transform.position;

#if UNITY_EDITOR
            SceneVisibilityManager.instance.DisablePicking(itemInGridSpace, true);

            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(itemInGridSpace); // Marks it as modified
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(itemInGridSpace);
            }
#endif
        }

        public void RemoveTrack()
        {
            GameObject.Destroy(itemInGridSpace);
            itemInGridSpace = null;
        }
    }
}
