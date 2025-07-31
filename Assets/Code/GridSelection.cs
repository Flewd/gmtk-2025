using UnityEngine;

namespace Code
{
    public class GridSelection : MonoBehaviour
    {
        public LayerMask selectionLayerMask;
        
        public GridItem selected;
        public Transform selectionCursor;
        
        private RaycastHit[] hits = new RaycastHit[32];
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out RaycastHit hit, 100, selectionLayerMask))
                {
                    Transform objectHit = hit.transform;
                    if(objectHit.TryGetComponent(out GridItem item))
                    {
                        selected = item;
                        selectionCursor.position = selected.transform.position + Vector3.up;
                        
                        selected.SetItemInGridSpace(GridItem.ItemType.track);
                    }
                }
            }
            
            if(Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out RaycastHit hit, 100, selectionLayerMask))
                {
                    Transform objectHit = hit.transform;
                    if(objectHit.TryGetComponent(out GridItem item))
                    {
                        selected = item;
                        selectionCursor.position = selected.transform.position + Vector3.up;
                        
                        selected.SetItemInGridSpace(GridItem.ItemType.empty);
                    }
                }
            }
        }
    }
}
