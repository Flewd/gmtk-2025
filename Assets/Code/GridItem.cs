using TMPro;
using UnityEngine;

namespace Code
{
    public class GridItem : MonoBehaviour
    {
        public enum ItemType { empty, track }
        public GameObject itemInGridSpace;
        public TMP_Text text;
        public ItemType currentItemType = ItemType.empty;
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
                        GameObject.Destroy(itemInGridSpace);
                    }
                    
                    break;
                case ItemType.track:
                    if (currentItemType != ItemType.track)
                    {
                        itemInGridSpace = Instantiate(Resources.Load("Track") as GameObject);
                        itemInGridSpace.transform.position = transform.position;
                    }
                    
                    break;
            }


            currentItemType = itemType;
        }
    }
}
