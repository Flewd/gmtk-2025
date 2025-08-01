using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor( typeof(GridItem) )]
    public class GridItemEditor: UnityEditor.Editor
    {
        [MenuItem( "Tools/Toggle Track #q" )]
        public static void ToggleTrack()
        {
            var gameObjects = Selection.gameObjects;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].TryGetComponent(out GridItem item))
                {
                    if (item.currentItemType == GridItem.ItemType.track)
                    {
                        item.currentItemType = GridItem.ItemType.empty;
                        GameObject.DestroyImmediate(item.itemInGridSpace);
                        item.itemInGridSpace = null;
                    }
                    else if (item.currentItemType == GridItem.ItemType.empty)
                    {
                        item.SetItemInGridSpace(GridItem.ItemType.track);
                    }
                }
            }
        }
        
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var item = target as GridItem;
            
            if (item.currentItemType == GridItem.ItemType.track &&
                item.itemInGridSpace == null)
            {
                item.SpawnTrack();
            }
            else if (item.currentItemType == GridItem.ItemType.empty &&
                item.itemInGridSpace != null)
            {
                GameObject.DestroyImmediate(item.itemInGridSpace);
                item.itemInGridSpace = null;
            }
        }
    }
}