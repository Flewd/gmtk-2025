using System;
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
            if (!Application.isPlaying)
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
                            EditorUtility.SetDirty(item);
                        }
                        else if (item.currentItemType == GridItem.ItemType.empty)
                        {
                            item.SetItemInGridSpace(GridItem.ItemType.track);
                            EditorUtility.SetDirty(item);
                        }
                    }
                }
            }
        }

        public void OnValidate()
        {
            
        }

        public override void OnInspectorGUI()
        {
            Debug.Log("OnInspectorGUI");
            DrawDefaultInspector();
            if (!Application.isPlaying)
            {
                var item = target as GridItem;
            
                if (item.currentItemType == GridItem.ItemType.track &&
                    item.itemInGridSpace == null)
                {
                    item.SpawnTrack();
                }
                else if (item.currentItemType == GridItem.ItemType.empty )
                {
                    var tracks = item.GetComponentsInChildren<Track>();
                    foreach (var track in tracks)
                    {
                        GameObject.DestroyImmediate(track.gameObject);
                    }
                    item.itemInGridSpace = null;
                }
            }
        }
    }
}